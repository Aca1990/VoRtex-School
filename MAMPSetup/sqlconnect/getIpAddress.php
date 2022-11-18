<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	$user_id = mysqli_real_escape_string($con,$_POST["user_id"]); //$_POST["user_id"];
	$user_idclean = filter_var($user_id, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	if(($user_id != $user_idclean))
	{
		echo "7: text do not match, maybe SQL injection issue"; // error code #7 - text do not match
		exit();
	}

	// check if name exists
	$namecheckquery = "SELECT address FROM hostingaddresses WHERE user_id='". $user_id . "';";
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $user_id); // error code #2 - name check query failed

	$publicIP = mysqli_real_escape_string($con,$_POST["user_ip"]); //$_POST["user_id"];
	$publicIP = filter_var($publicIP, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);	
	
	//$localIP = getHostByName(getHostName());
	//$getPublicIP =file_get_contents('http://checkip.dyndns.com/');
	//preg_match("/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/", $getPublicIP, $publicIPAdrdresses);
	//$publicIP = $publicIPAdrdresses[0];
	
	if(mysqli_num_rows($namecheck) != 1)
	{
	    $exe_query = "INSERT INTO hostingaddresses (hosting_id, user_id, address) VALUES (NULL, '". $user_id ."', '". $publicIP ."');";
		$query = mysqli_query($con, $exe_query)or die($exe_query);
	}
	else
	{
		// get loggin info
		$loginInfo = mysqli_fetch_assoc($namecheck);
		
		if($publicIP != $loginInfo["address"])
		{
			$exe_query = "UPDATE hostingaddresses SET address = '$publicIP' WHERE hostingaddresses.user_id = '$user_id';";
			$query = mysqli_query($con, $exe_query)or die("UPDATE query failed " . $exe_query);
		}
	}
	echo "0\t" . $publicIP;
?>