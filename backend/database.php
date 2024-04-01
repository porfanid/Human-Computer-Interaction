<?php

require 'vendor/autoload.php'; // Include Firebase PHP Admin SDK

use Google\Cloud\Firestore\FirestoreClient;

// Initialize Firestore client
$firestore = new FirestoreClient([
    'projectId' => 'human-computer-int',
    'keyFilePath' => 'google-services.json'
]);

// Reference to your Firestore collection
$collection = $firestore->collection('Medicine UOI Museum');

// Get all documents from the collection
$documents = $collection->documents();

// Convert documents to array
$data = [];
foreach ($documents as $document) {
    $data[] = $document->data();
}

// Convert data to JSON
$jsonData = json_encode($data);

// Output JSON
header('Content-Type: application/json');
echo $jsonData;

?>