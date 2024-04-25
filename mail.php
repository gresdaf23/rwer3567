<?php
$to = "ivan-max47@yandex.ru";
$subject = "Сообщение с сайта портфолио";
$from = "From: " . $_POST['c_name'] . '<' . $_POST['c_email'] . '>';
$message = $_POST['c_message'];
if (mail($to, $subject, $message, $from)){
    echo "Письмо отправлено<br>";
} else{
    echo "Ошибка при отправке письма<br>";
}
$connection = mysqli_connect('localhost', 'root', '32167', 'portfolio_db');
mysqli_set_charset($connection, 'utf8');
if (mysqli_query($connection, "INSERT INTO `mail` VALUES (NULL, '".$_POST['c_name']."', '".$_POST['c_email']."', '".$_POST['c_message']."')")){
    echo "Сообщение занесено в базу данных<br>";
} else{
    echo "Записать сообщение не удалось<br>";
}
mysqli_close($connection);