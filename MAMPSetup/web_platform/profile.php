<?php
$con_users = mysqli_connect("localhost", "root", "root", "useraccess");

include("includes/header.php");
require 'config/config.php';
require 'includes/form_handlers/microlesson_handler.php';
require 'includes/form_handlers/user_handler.php';

$message_obj = new Message($con, $userLoggedIn);

if(isset($_GET['profile_username'])) {
	$username = $_GET['profile_username'];
	$user_details_query = mysqli_query($con, "SELECT * FROM users WHERE username='$username'");
	$user_array = mysqli_fetch_array($user_details_query);
    $user_email = $user_array['email'];
	$num_friends = (substr_count($user_array['friend_array'], ",")) - 1;
}



if(isset($_POST['remove_friend'])) {
	$user = new User($con, $userLoggedIn);
	$user->removeFriend($username);
}

if(isset($_POST['add_friend'])) {
	$user = new User($con, $userLoggedIn);
	$user->sendRequest($username);
}
if(isset($_POST['respond_request'])) {
	header("Location: requests.php");
}

if(isset($_POST['post_message'])) {
  if(isset($_POST['message_body'])) {
    $body = mysqli_real_escape_string($con, $_POST['message_body']);
    $date = date("Y-m-d H:i:s");
    $message_obj->sendMessage($username, $body, $date);
  }

  $link = '#profileTabs a[href="#messages_div"]';
  echo "<script> 
          $(function() {
              $('" . $link ."').tab('show');
          });
        </script>";


}

 ?>

 	<style type="text/css">
	 	.wrapper {
	 		margin-left: 0px;
			padding-left: 0px;
	 	}

 	</style>
	
 	<div class="profile_left">
 		<img src="<?php echo $user_array['profile_pic']; ?>">

 		<div class="profile_info">
 			<p><?php echo "Posts: " . $user_array['num_posts']; ?></p>
 			<p><?php echo "Likes: " . $user_array['num_likes']; ?></p>
 			<p><?php echo "Friends: " . $num_friends ?></p>
 		</div>

 		<form action="<?php echo $username; ?>" method="POST">
 			<?php 
 			$profile_user_obj = new User($con, $username); 
 			if($profile_user_obj->isClosed()) {
 				header("Location: user_closed.php");
 			}

 			$logged_in_user_obj = new User($con, $userLoggedIn); 

 			if($userLoggedIn != $username) {

 				if($logged_in_user_obj->isFriend($username)) {
 					echo '<input type="submit" name="remove_friend" class="danger" value="Remove Friend"><br>';
 				}
 				else if ($logged_in_user_obj->didReceiveRequest($username)) {
 					echo '<input type="submit" name="respond_request" class="warning" value="Respond to Request"><br>';
 				}
 				else if ($logged_in_user_obj->didSendRequest($username)) {
 					echo '<input type="submit" name="" class="default" value="Request Sent"><br>';
 				}
 				else 
 					echo '<input type="submit" name="add_friend" class="success" value="Add Friend"><br>';

 			}

 			?>
 		</form>
 		<input type="submit" class="deep_blue" data-toggle="modal" data-target="#post_form" value="Post Something">

    <?php  
    if($userLoggedIn != $username) {
      echo '<div class="profile_info_bottom">';
        echo $logged_in_user_obj->getMutualFriends($username) . " Mutual friends";
      echo '</div>';
    }
    ?>

 	</div>


	<div class="profile_main_column column">

    <ul class="nav nav-tabs" role="tablist" id="profileTabs">
      <li role="presentation" class="active"><a href="#newsfeed_div" aria-controls="newsfeed_div" role="tab" data-toggle="tab">Newsfeed</a></li>
      <li role="presentation"><a href="#messages_div" aria-controls="messages_div" role="tab" data-toggle="tab">Messages</a></li>
	  <li role="presentation"><a href="#content_div" aria-controls="content_div" role="tab" data-toggle="tab">Create Content</a></li>
	  <li role="presentation"><a href="#students_div" aria-controls="students_div" role="tab" data-toggle="tab">Students progress</a></li>
	  <li role="presentation"><a href="#user_div" aria-controls="user_div" role="tab" data-toggle="tab">User settings</a></li>
    </ul>

    <div class="tab-content">

      <div role="tabpanel" class="tab-pane fade in active" id="newsfeed_div">
        <div class="posts_area"></div>
        <img id="loading" src="assets/images/icons/loading.gif">
      </div>


      <div role="tabpanel" class="tab-pane fade" id="messages_div">
        <?php  
        

          echo "<h4>Messages from <a href='" . $username ."'>" . $profile_user_obj->getFirstAndLastName() . "</a></h4><hr><br>";

          echo "<div class='loaded_messages' id='scroll_messages'>";
            echo $message_obj->getMessages($username);
          echo "</div>";
        ?>

        <div class="message_post">
          <form action="" method="POST">
              <textarea name='message_body' id='message_textarea' placeholder='Write your message ...'></textarea>
              <input type='submit' name='post_message' class='info' id='message_submit' value='Send'>
          </form>

        </div>

        <script>
          var div = document.getElementById("scroll_messages");
          div.scrollTop = div.scrollHeight;
        </script>
      </div>

	  <div role="tabpanel" class="tab-pane fade" id="content_div">
        <?php  
        

          echo "<h4>Create/Select content</h4><hr><br>";

        ?>

        <div class="message_post">
			<form action="<?php echo $username; ?>" method="POST">
				<?php
					$sql_user = "SELECT users.id,users.role_id,users.microlesson_id  FROM users WHERE users.webplatform_email='". $user_email . "';";
					$result = mysqli_query($con_users, $sql_user);
					$userInfo = mysqli_fetch_assoc($result); // role id equal 1 can use platform	
					$user_id = $userInfo["id"];
					$role_id = intval($userInfo["role_id"]);
					$active_microlesson = $userInfo["microlesson_id"];
					
					$sql_lessons = "SELECT microlesson_id, lesson_name FROM microlessons WHERE user_id='". $user_id . "';";
					$sql_lessons_all = "SELECT microlesson_id, lesson_name FROM microlessons;";
					
					if($role_id==1)
					{
						echo "<p>Lesson Name</p>";
					}
				?>

				<input <?php echo $role_id>1 ? 'type="hidden"' : 'type="text"'; ?> name="microlesson_name" placeholder="Enter Name" value="<?php 
				if(isset($_SESSION['microlesson_name'])) {
					echo $_SESSION['microlesson_name'];
				}
				?>" required>
				<br>
				<?php if(in_array("Microlesson name must be between 2 and 25 characters<br>", $error_array_ml)) echo "Microlesson name must be between 2 and 25 characters<br>"; ?>		
				<br>
				
				<input <?php echo $role_id>1 ? 'type="hidden"' : 'type="checkbox"'; ?> id="presentation_on" name="presentation_on" value="presentation_on" checked>
				<?php
					if($role_id==1)
					{
						echo "<label for='presentation_on'>Presentation</label>";
						echo "<br>";
					}
				?>
				
				<input <?php echo $role_id>1 ? 'type="hidden"' : 'type="checkbox"'; ?> id="interactables_on" name="interactables_on" value="interactables_on">
				<?php
					if($role_id==1)
					{
						echo "<label for='interactables_on'>Interactables</label>";
						echo "<br>";
					}
				?>
				
				<input type="hidden" id="userId" name="userId" value="<?php echo $user_id; ?>">
	
				<?php
				if($role_id==1)
				{
					echo "<p>Lesson Environment</p>";
					$sql_le = "SELECT lesson_environment_id, Name FROM lesson_environment;";
					$result = mysqli_query($con_users, $sql_le);

					echo "<select name='Environment'>";
					while ($row = mysqli_fetch_array($result)) {
					   echo "<option value='" .$row['lesson_environment_id']."'> ".$row['Name'] . "</option>"; 
					}
					echo "</select>";
					echo "<br>";
					echo "<br>";
				}
				?>
 
				<?php
				if($role_id==1)
				{
					echo "<p>Study Group</p>";
					$sql_sg = "SELECT study_group_id, Name FROM study_group;";
					$result = mysqli_query($con_users, $sql_sg);

					echo "<select name='Group'>";
					while ($row = mysqli_fetch_array($result)) {
					   echo "<option value='" .$row['study_group_id']."'> ".$row['Name'] . "</option>"; 
					}
					echo "</select>";
					echo "<br>";
					echo "<br>";
				}
				?>

				<input <?php echo $role_id>1 ? 'type="hidden"' : 'type="submit"'; ?> name="create_lesson" value="Create lesson">
				<br>

				<?php if(in_array("<span style='color: #14C800;'>Microlesson Added!</span><br>", $error_array_ml)) echo "<span style='color: #14C800;'>You're all set! Go ahead and select Micro Lesson!</span><br>"; ?>
			</form>
				<?php
				if($role_id>1)
				{
					$result_lessons = mysqli_query($con_users, $sql_lessons_all); // student
				}
				else
				{
					$result_lessons = mysqli_query($con_users, $sql_lessons); // teacher
				}
				 if (mysqli_num_rows($result_lessons) != 0) {
					echo "<form action='". $username . "' method='POST'>";
					echo "<input type='hidden' id='userId' name='userId' value='". $user_id . "'>";
					echo "<p>Select Lesson</p>";

					echo "<select name='Lessons'>";
					while ($row = mysqli_fetch_array($result_lessons)) {
							if ($active_microlesson != $row['microlesson_id'])
								echo "<option value='" .$row['microlesson_id']."'> ".$row['lesson_name'] . "</option>";
							else
								echo "<option value='" .$row['microlesson_id']."' selected> ".$row['lesson_name'] . "</option>";
						}
						echo "</select>";
						
						echo "<br>";
						echo "<br>";

						echo "<input type='submit' name='select_lesson' value='Select lesson'>";
						echo "<br>";
						echo "</form>";
				}
				?>
        </div>

      </div>
	  
	  <div role="tabpanel" class="tab-pane fade" id="students_div">
        <?php  
        
			if($role_id==1)
			{
				echo "<h4>Students progress</h4><hr><br>";
			}
			else
			{
				echo "<h4>Your progress</h4><hr><br>";			
			}

        ?>
		
        <div class="message_post">
			<?php
			if($role_id==1)
			{
				echo "<h2>Student list</h2>";
				$sql_students = "SELECT users.username,users.achievements,microlessons.lesson_name FROM users INNER JOIN microlessons ON users.microlesson_id=microlessons.microlesson_id WHERE users.role_id='2';";
				$result_students = mysqli_query($con_users, $sql_students)or die($sql_students);

				while ($row = mysqli_fetch_array($result_students)) {
				   	$username_student = $row["username"];
					$achievements = $row["achievements"];
					$lesson_name = $row["lesson_name"];
					echo "<b>User Name</b>";
					echo "<p>$username_student</p>";			
					echo "<b>Achievements</b>";
					echo "<p>$achievements</p>";				
					echo "<b>Microlesson</b>";
					echo "<p>$lesson_name</p>";					
					echo "<br>";
					echo "<br>";
				}
				echo "<br>";
			}
            else
            {
				$sql_students = "SELECT users.username,users.achievements,microlessons.lesson_name FROM users INNER JOIN microlessons ON users.microlesson_id=microlessons.microlesson_id WHERE users.id='". $user_id . "';";
				$result_students = mysqli_query($con_users, $sql_students)or die($sql_students);

                $loginInfo = mysqli_fetch_assoc($result_students);
                $usernameclient = $loginInfo["username"];
                $achievements = $loginInfo["achievements"];
                $lesson_name = $loginInfo["lesson_name"];
                echo "<b>User Name</b>";
                echo "<p>$usernameclient</p>";			
                echo "<b>Achievements</b>";
                echo "<p>$achievements</p>";				
                echo "<b>Microlesson</b>";
                echo "<p>$lesson_name</p>";					
                echo "<br>";
				echo "<br>";                
            }
			?>
        </div>

      </div>
	  
	  <div role="tabpanel" class="tab-pane fade" id="user_div">
        <?php  
        
			echo "<h4>User Settings</h4><hr><br>";

        ?>
		
        <div class="message_post">
			<form action="<?php echo $username; ?>" method="POST">
				<?php
					$sql_students = "SELECT avatar_type.name FROM avatar_type INNER JOIN users ON users.avatar_type_id=avatar_type.avatar_type_id WHERE users.id='$user_id';";
				    $result_students = mysqli_query($con_users, $sql_students)or die($sql_students);
					$loginInfo = mysqli_fetch_assoc($result_students);
					$avatar_name = $loginInfo["name"];
					
					echo "<h2>Avatar type</h2>";
				
					echo "<p>Selected avatar is $avatar_name</p>";
					$sql_le = "SELECT avatar_type_id, name FROM avatar_type;";
					$result = mysqli_query($con_users, $sql_le);

					echo "<select name='Avatar'>";
					while ($row = mysqli_fetch_array($result)) {
					   echo "<option value='" .$row['avatar_type_id']."'> ".$row['name'] . "</option>"; 
					}
					echo "</select>";
					echo "<br>";
					echo "<br>";
				?>
				<input type="hidden" id="userId" name="userId" value="<?php echo $user_id; ?>">
				<input type="submit" name="select_avatar" value="Select avatar">
			</form>
        </div>

      </div>

    </div>

	</div>

<!-- Modal -->
<div class="modal fade" id="post_form" tabindex="-1" role="dialog" aria-labelledby="postModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">

      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="postModalLabel">Post something!</h4>
      </div>

      <div class="modal-body">
      	<p>This will appear on the user's profile page and also their newsfeed for your friends to see!</p>

      	<form class="profile_post" action="" method="POST">
      		<div class="form-group">
      			<textarea class="form-control" name="post_body"></textarea>
      			<input type="hidden" name="user_from" value="<?php echo $userLoggedIn; ?>">
      			<input type="hidden" name="user_to" value="<?php echo $username; ?>">
      		</div>
      	</form>
      </div>

      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" name="post_button" id="submit_profile_post">Post</button>
      </div>
    </div>
  </div>
</div>


<script>
  var userLoggedIn = '<?php echo $userLoggedIn; ?>';
  var profileUsername = '<?php echo $username; ?>';

  $(document).ready(function() {

    $('#loading').show();

    //Original ajax request for loading first posts 
    $.ajax({
      url: "includes/handlers/ajax_load_profile_posts.php",
      type: "POST",
      data: "page=1&userLoggedIn=" + userLoggedIn + "&profileUsername=" + profileUsername,
      cache:false,

      success: function(data) {
        $('#loading').hide();
        $('.posts_area').html(data);
      }
    });

    $(window).scroll(function() {
      var height = $('.posts_area').height(); //Div containing posts
      var scroll_top = $(this).scrollTop();
      var page = $('.posts_area').find('.nextPage').val();
      var noMorePosts = $('.posts_area').find('.noMorePosts').val();

      if ((document.body.scrollHeight == document.body.scrollTop + window.innerHeight) && noMorePosts == 'false') {
        $('#loading').show();

        var ajaxReq = $.ajax({
          url: "includes/handlers/ajax_load_profile_posts.php",
          type: "POST",
          data: "page=" + page + "&userLoggedIn=" + userLoggedIn + "&profileUsername=" + profileUsername,
          cache:false,

          success: function(response) {
            $('.posts_area').find('.nextPage').remove(); //Removes current .nextpage 
            $('.posts_area').find('.noMorePosts').remove(); //Removes current .nextpage 

            $('#loading').hide();
            $('.posts_area').append(response);
          }
        });

      } //End if 

      return false;

    }); //End (window).scroll(function())


  });

  </script>





	</div>
</body>
</html>