<?php

	$con = mysqli_connect("localhost", "root", "root", "useraccess");

	//check connection
	if(mysqli_connect_errno())
	{
		echo "1: connection failed"; // error code failed
		exit();
	}

	// check if name exists
	// Read the image bytes into the $data variable

	$ImagePath = "C:\\Users\\acajo\\AppData\\LocalLow\\DefaultCompany\\VortexPrototype\\virtual_reality";

	for ($i = 1; $i <= 38; $i++) {
		$imageName = "slide$i.png";
		$imageLocation = $ImagePath . "\\" . $imageName;
		$data = base64_encode(file_get_contents($imageLocation));
		
		//$fp = "1".$imageName;
		//file_put_contents($fp, $data);
		//echo $imageLocation;

		// Create the query
		// imgTitle	imgType	imgData	microlesson_id
		$presentationInsert = "INSERT INTO presentation VALUES (null, '$imageName', 'png', '$data', '1');"; // INSERT INTO presentation VALUES (null, "slika1.png", 'png', null, '1');
		// Execute the query    
		mysqli_query($con, $presentationInsert) or die("4: insert player query failed");
	}

	echo "0\t";
?>