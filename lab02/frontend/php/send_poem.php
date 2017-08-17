<?php

    const SELF_HOSTED_APPLICATION_URL = "http://localhost:9000/api/values/";
    const SHOW_VALUE_URL = "/php/get_value.php";
    const HTTP_STATUS_OK = 200;

    $value = (isset($_POST["value"])) ? $_POST["value"] : null;
    if ($value == null)
    {
        die("No value entered...");
    }

    $data = [
        "id"    => "0",
        "value" => $value
    ];

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, SELF_HOSTED_APPLICATION_URL);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HEADER, false);
    curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($data));

    $response = curl_exec($ch);
    $resultStatus = curl_getinfo($ch, CURLINFO_HTTP_CODE);

    var_dump($resultStatus);

//    if($resultStatus != HTTP_STATUS_OK)
//    {
//        die("Could not save values...");
//    }
//
//    header("Location: " . SHOW_VALUE_URL . "?id=0");