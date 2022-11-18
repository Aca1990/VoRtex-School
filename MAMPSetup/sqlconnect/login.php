<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	$username = mysqli_real_escape_string($con,$_POST["name"]); //$_POST["name"];
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	$password = mysqli_real_escape_string($con,$_POST["password"]); //$_POST["password"];
	$passwordclean = filter_var($password, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	if(($username != $usernameclean) && ($password != $passwordclean))
	{
		echo "7: text do not match, maybe SQL injection issue"; // error code #7 - text do not match
		exit();
	}

	// check if name exists
	$namecheckquery = "SELECT users.username, users.salt, users.hash, users.achievements, users.face_recognition_image, users.role_id, users.id, users.microlesson_id from users WHERE users.username='". $username . "';";
	# $namecheckquery = "SELECT users.username, users.salt, users.hash, users.achievements, users.face_recognition_image, users.role_id, users.id, microlessons.lesson_name FROM users INNER JOIN microlessons ON users.id=microlessons.user_id WHERE users.username='". $username . "';";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $username); // error code #2 - name check query failed

	if(mysqli_num_rows($namecheck) != 1)
	{
		echo "5: name do not exists"; // error code #5 - name do not exists
		exit();
	}

	// get loggin info
	$loginInfo = mysqli_fetch_assoc($namecheck);
	$salt = $loginInfo["salt"];
	$hash = $loginInfo["hash"];

	//$salt = "\$5\$rounds=5000\$" . "something" . $username . "\$";
	$loginHash = crypt($password, $salt);
	if($hash != $loginHash)
	{
		echo "6: Incorrect password"; // error code #6 - Incorrect password
		exit();
	}
	
	$microlessoncheckquery = "SELECT microlessons.lesson_name, microlessons.lesson_environment_id, microlessons.study_group_id FROM microlessons WHERE microlessons.microlesson_id='". $loginInfo["microlesson_id"] . "';";
	$microlessonnamecheck = mysqli_query($con, $microlessoncheckquery) or die("2: microlesson check query failed " . $username); // error code #2 - name check query failed
	$microlessonInfo = mysqli_fetch_assoc($microlessonnamecheck);

	echo "0\t" . $loginInfo["achievements"] . "\t" . $loginInfo["face_recognition_image"] . "\t" . $loginInfo["role_id"] . "\t" . $loginInfo["id"] . "\t" . $microlessonInfo["lesson_name"] . "\t" . $microlessonInfo["lesson_environment_id"] . "\t" . $loginInfo["microlesson_id"];
?>