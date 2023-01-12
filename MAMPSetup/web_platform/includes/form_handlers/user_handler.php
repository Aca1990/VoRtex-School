<?php

//Declaring variables to prevent errors
$user_id = ""; // user id
$avatar_type_id; // selected avatar

if(isset($_POST['select_avatar'])){
	//Avatar id
	$avatar_type_id = strip_tags($_POST['Avatar']); //Remove html tags
	$avatar_type_id = str_replace(' ', '', $avatar_type_id); //remove spaces
	$_SESSION['Avatar'] = $avatar_type_id; //Stores into session variable
	
	//user id
	$user_id = strip_tags($_POST['userId']); //Remove html tags
	$user_id = str_replace(' ', '', $user_id); //remove spaces
	$_SESSION['userId'] = $user_id; //Stores into session variable
	
	$exe_query = "UPDATE users SET avatar_type_id= '$avatar_type_id' WHERE users.id ='$user_id';";
	$query = mysqli_query($con_users, $exe_query)or die($exe_query);	
	
	$_SESSION['Avatar'] = "";
}
?>