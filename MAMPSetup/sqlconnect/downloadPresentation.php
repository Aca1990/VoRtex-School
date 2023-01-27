<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	$microlesson_id = mysqli_real_escape_string($con,$_POST["microlesson_id"]);
	$microlesson_idclean = filter_var($microlesson_id, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	if($microlesson_id != $microlesson_idclean)
	{
		echo "7: text do not match, maybe SQL injection issue"; // error code #7 - text do not match
		exit();
	}

	// check if name exists
	$namecheckquery = "SELECT presentation.microlesson_id, presentation.imgTitle, presentation.imgType, presentation.imgData FROM presentation WHERE presentation.microlesson_id='1' OR presentation.microlesson_id='". $microlesson_id . "';";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: name check query failed " . $microlesson_id); // error code #2 - name check query failed
    $count = mysqli_num_rows($namecheck);

	$images = "0\n";
	while ($row = mysqli_fetch_array($namecheck)) {
		$imgtitle = $row["imgTitle"];
		$imgtype = $row["imgType"];
		$blob = $row["imgData"];
		$images = $images . "$imgtitle\t$blob" . "\n";
	}
	
	echo $images;
?>