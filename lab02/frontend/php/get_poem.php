<?php

    const SELF_HOSTED_APPLICATION_URL = "http://localhost:9000/api/values/";

    $id = (isset($_GET["id"])) ? stripcslashes($_GET["id"]) : null;
    if ($id == null)
    {
        die("No id ...");
    }
    $request = SELF_HOSTED_APPLICATION_URL . $id;

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $request);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HEADER, false);

    $response = curl_exec($ch);
    if(!$response)
    {
        die("Could not get values...");
    }

    $response = str_replace('"', '', $response);
    $responseByLines = explode('\n', $response);

    foreach ($responseByLines as $line)
    {
        echo "$line" . "<br>";
    }
?>
