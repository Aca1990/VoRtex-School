<?php

//Declaring variables to prevent errors
$environment = ""; //Environment name
$group = ""; //Study group name
$name = ""; //Microlesson name
$user_id = ""; // user id
$microlesson_id; // selected microlesson
$error_array_ml = array(); //Holds error messages

if(isset($_POST['create_lesson'])){

	//Environment name
	$environment = strip_tags($_POST['Environment']); //Remove html tags
	$environment = str_replace(' ', '', $environment); //remove spaces
	$_SESSION['Environment'] = $environment; //Stores into session variable

	//Study group name
	$group = strip_tags($_POST['Group']); //Remove html tags
	$group = str_replace(' ', '', $group); //remove spaces
	$_SESSION['Group'] = $group; //Stores into session variable

	//name
	$name = strip_tags($_POST['microlesson_name']); //Remove html tags
	$name = str_replace(' ', '', $name); //remove spaces
	$_SESSION['microlesson_name'] = $name; //Stores into session variable
	
	//user id
	$user_id = strip_tags($_POST['userId']); //Remove html tags
	$user_id = str_replace(' ', '', $user_id); //remove spaces
	$_SESSION['userId'] = $user_id; //Stores into session variable

	if(strlen($name) > 25 || strlen($name) < 2) {
		array_push($error_array_ml, "Microlesson name must be between 2 and 25 characters<br>");
	}

	if(empty($error_array_ml)) {
		//Generate username by concatenating first name and last name
		$check_name_query = mysqli_query($con_users, "SELECT lesson_name FROM microlessons WHERE lesson_name='$name'");

		$i = 0; 
		//if username exists add number to username
		while(mysqli_num_rows($check_name_query) != 0) {
			$i++; //Add 1 to i
			$name = $name . "_" . $i;
			$check_name_query = mysqli_query($con_users, "SELECT lesson_name FROM microlessons WHERE lesson_name='$name'");
		}

        $exe_query = "INSERT INTO microlessons VALUES (null, '$name', null, '$user_id', '$environment', '$group')";
		$query = mysqli_query($con_users, $exe_query)or die($exe_query);

		array_push($error_array_ml, "<span style='color: #14C800;'>Microlesson Added!</span><br>");

		//Clear session variables 
		$_SESSION['Environment'] = "";
		$_SESSION['Group'] = "";
		$_SESSION['microlesson_name'] = "";
		$_SESSION['userId'] = "";
	}

}

if(isset($_POST['select_lesson'])){
	//Microlesson id
	$microlesson_id = strip_tags($_POST['Lessons']); //Remove html tags
	$microlesson_id = str_replace(' ', '', $microlesson_id); //remove spaces
	$_SESSION['Lessons'] = $microlesson_id; //Stores into session variable
	
	//user id
	$user_id = strip_tags($_POST['userId']); //Remove html tags
	$user_id = str_replace(' ', '', $user_id); //remove spaces
	$_SESSION['userId'] = $user_id; //Stores into session variable
	
	$exe_query = "UPDATE users SET microlesson_id= '$microlesson_id' WHERE users.id ='$user_id';";
	$query = mysqli_query($con_users, $exe_query)or die($exe_query);	
	
	$_SESSION['Lessons'] = "";
}
?>