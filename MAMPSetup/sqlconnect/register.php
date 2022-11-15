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
	$namecheckquery = "SELECT username FROM users WHERE username='". $username . "';";
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $username); // error code #2 - name check query failed

	if(mysqli_num_rows($namecheck) > 0)
	{
		echo "3: name already exists"; // error code #3 - name exists cannot register
		exit();
	}

	// add user to the table
	$salt = "\$5\$rounds=5000\$" . "something" . $username . "\$";
	$hash = crypt($password, $salt);

	$insertuserquery = "INSERT INTO users (username, hash, salt, role_id) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "' , " . 2 . ");";
	mysqli_query($con, $insertuserquery) or die("4: insert player query failed"); // error code #4 - insert query failed
	echo("0");
?>