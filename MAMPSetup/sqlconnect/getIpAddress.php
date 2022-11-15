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
	$namecheckquery = "SELECT address FROM hostingaddresses WHERE user_id=". $user_id . ";";
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $user_id); // error code #2 - name check query failed

	if(mysqli_num_rows($namecheck) != 1)
	{
		echo "5: name do not exists"; // error code #5 - name do not exists
		exit();
	}
	// get loggin info
	$loginInfo = mysqli_fetch_assoc($namecheck);
	
	echo $loginInfo["address"];
?>