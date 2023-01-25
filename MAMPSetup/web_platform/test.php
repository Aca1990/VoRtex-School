<head>
<STYLE>
<!-- 
#mid p {color:#555;}
#mid {background-color:#f4f4f4;float:center;width:750px;padding:5px;margin:0 auto;}
#mid h1 {font:250% "Arial";font-weight:bold;color:#d75900;margin:auto;padding:5px 0px 0px 0px;}
#mid h3 {font:120% "Arial";color:#009600;margin:auto;padding:0px 0px 0px 0px;}
-->
</STYLE> 
</head>
<body>
  <? 
    $yourdomain = $_SERVER['HTTP_HOST'];
    $yourdomain = preg_replace('/^www\./' , '' , $yourdomain);
    ?>

<div id="mid">
<h1><? echo "$yourdomain" ;?></h1>
<p><?echo gmdate ("M d Y H:i:s",time());?></p>
<h3>your domain is online and this test file shows your hosting php ini</h3>
</div>
<?php
phpinfo ();
?>
</body>
