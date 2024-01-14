import React from "react";
import { useState, useEffect } from "react";
import authService from "./api-authorization/AuthorizeService";

function UpdateDatabase() {
    const [response1, setResponse1] = useState(null);
    const [response2, setResponse2] = useState(null);

    useEffect(() => {
        authService.getAccessToken().then((token) => {
            fetch("csvtodatabase", {
                headers: !token ? {} : { Authorization: `Bearer ${token}` },
            }).then((response) => {
                response.text().then((data) => {
                    setResponse1(data);
                });
            });
        });
        authService.getAccessToken().then((token) => {
            fetch("weatherapidatatodatabase", {
                headers: !token ? {} : { Authorization: `Bearer ${token}` },
            }).then((response) => {
                response.text().then((data) => {
                    setResponse2(data);
                });
            });
        });
    }, []);

    return (
        <div>
            <h1>Uploading csv to database</h1>
            <p>It usually takes a lot seconds.</p>
            {response1 ?? "Please wait..."}
            <h1>Uploading api data to database</h1>
            <p>It usually takes 20 seconds.</p>
            {response2 ?? "Please wait..."}
        </div>
    );
}

export default UpdateDatabase;
