<?php
ob_start(); //Turns on output buffering 
session_start();

$timezone = date_default_timezone_set("Europe/London");

$con = mysqli_connect("localhost", "root", "root", "social"); //Connection variable

if(mysqli_connect_errno()) 
{
	echo "Failed to connect: " . mysqli_connect_errno();
}

$con_users = mysqli_connect("localhost", "root", "root", "useraccess");
//check connection
if(mysqli_connect_errno())
{
	echo "1: connection failed"; // error code failed
	exit();
}

?>