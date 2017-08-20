<?php

    const SELF_HOSTED_APPLICATION_URL = "http://localhost:9000/api/values/";
    const SHOW_VALUE_URL = "/php/get_poem.php";
    const HTTP_STATUS_OK = 200;

    $value = (isset($_POST["value"])) ? $_POST["value"] : null;
    if ($value == null)
    {
        die("No value entered...");
    }

    $textByLines = explode("\n", $value);

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, SELF_HOSTED_APPLICATION_URL);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HEADER, false);

    $id = 1;
    foreach ($textByLines as $line)
    {
        $data = [
            "id"    => $id,
            "count" => count($textByLines),
            "value" => $line
        ];
        curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($data));
        $response = curl_exec($ch);
        $id++;
    }
    $resultStatus = curl_getinfo($ch, CURLINFO_HTTP_CODE);

    if($resultStatus != HTTP_STATUS_OK)
    {
        die("Could not save values...");
    }

    echo "Message has been sent";
    header("Location: " . SHOW_VALUE_URL . "?id=0");