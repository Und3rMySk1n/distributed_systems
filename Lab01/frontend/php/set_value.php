<?php

    const SELF_HOSTED_APPLICATION_URL = "http://localhost:9000/api/values/";
    const SHOW_VALUE_URL = "/php/get_value.php";

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
    if(!$response)
    {
        die("Could not save values...");
    }

    $response = str_replace('"', "", $response);
    header("Location: " . SHOW_VALUE_URL . "?id=" . $response);