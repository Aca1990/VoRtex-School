<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	$username = $_POST["name"];
	$achievements = $_POST["achievements"];

	// check if name exists
	$namecheckquery = "SELECT username, achievements FROM users WHERE username='". $username . "';";
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $username); // error code #2 - name check query failed

	if(mysqli_num_rows($namecheck) != 1)
	{
		echo "5: name do not exists"; // error code #5 - name do not exists
		exit();
	}

	// get loggin info
	$loginInfo = mysqli_fetch_assoc($namecheck);
	$existingAchievements = $loginInfo["achievements"];

	$listOfExistingAchievements = explode(",", $existingAchievements);


	if (in_array($achievements, $listOfExistingAchievements)) 
	{ 
		echo("1");
	}
	else
	{
        $updateQuery = "";
        if ($existingAchievements == null || empty($existingAchievements))
        {
		    $updateQuery = "UPDATE users SET achievements ='" . $achievements . "' WHERE username='". $username . "';";
        }
        else
        {
		    $updateQuery = "UPDATE users SET achievements ='" .$existingAchievements . "," . $achievements . "' WHERE username='". $username . "';";
        }
		mysqli_query($con, $updateQuery) or die("8: UPDATE query failed"); // error code #8 - UPDATE query failed
		echo("0");
	}

?>