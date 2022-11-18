<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	$microlesson_id = mysqli_real_escape_string($con,$_POST["microlesson_id"]); //$_POST["user_id"];
	$microlesson_idclean = filter_var($microlesson_id, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	if(($microlesson_id != $microlesson_idclean))
	{
		echo "7: text do not match, maybe SQL injection issue"; // error code #7 - text do not match
		exit();
	}

	// check if name exists
	$namecheckquery = "SELECT hostingaddresses.address FROM hostingaddresses INNER JOIN microlessons ON hostingaddresses.user_id=microlessons.user_id WHERE microlessons.microlesson_id='". $microlesson_id . "';";
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $user_id); // error code #2 - name check query failed
	
	// get loggin info
	$loginInfo = mysqli_fetch_assoc($namecheck);
	echo "0\t" . $loginInfo["address"];
?>