import { useState, useEffect } from "react";
import authService from "./api-authorization/AuthorizeService";
import ReactECharts from "echarts-for-react";
import React from "react";

function CloudCoverAverageAndCurrentPower(props) {
    const [response, setResponse] = useState(null);
    const [isLoading, setLoading] = useState(true);
  
    useEffect(() => {
        handleShowClick();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleShowClick = () => {
        authService.getAccessToken().then((token) => {
            setLoading(true);
            fetch(
                `monitoring/CloudCoverAverageAndCurrentPower?selectedDate=${encodeURIComponent(
                    props.selectedDate.toISOString()
                )}`,
                {
                    headers: !token ? {} : { Authorization: `Bearer ${token}` },
                }
            ).then((response) => {
                response.json().then((data) => {
                    setResponse(data);
                    setLoading(false);
                });
            });
        });
    };

    const options = {
        tooltip: {
            trigger: "axis",
            axisPointer: {
                type: "cross",
                crossStyle: {
                    color: "#999",
                },
            },
        },
        toolbox: {
            feature: {
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ["line", "bar"] },
                restore: { show: true },
                saveAsImage: { show: true },
            },
        },
        legend: {
            data: ["Total energy", "Cloud cover", "Energy average"],
        },
        xAxis: [
            {
                type: "category",
                data: response?.date.map(
                    (d) => `${new Date(d).getFullYear()}-${new Date(d).getUTCMonth() + 1}-${new Date(d).getDate()}`
                ),
                axisPointer: {
                    type: "shadow",
                },
                axisLabel: {
                    rotate: 45,
                },
            },
        ],
        yAxis: [
            {
                type: "value",
                name: "Cloud cover",
                min: 0,
                max: 100,
                interval: 25,
                axisLabel: {
                    formatter: "{value} %",
                },
            },
            {
                type: "value",
                name: "Energy",
                min: 0,
                max: response && Math.round(Math.max(...response.totalDayPower.map((x) => x.value))),
                interval: response && Math.round(Math.max(...response.totalDayPower.map((x) => x.value))) / 4,
                axisLabel: {
                    formatter: "{value} kWh",
                },
            },
        ],
        series: [
            {
                name: "Total energy",
                type: "bar",
                yAxisIndex: 1,
                tooltip: {
                    valueFormatter: function (value) {
                        return value + " kWh";
                    },
                },
                data: response?.totalDayPower,
            },
            {
                name: "Cloud cover",
                type: "line",
                yAxisIndex: 0,
                tooltip: {
                    valueFormatter: function (value) {
                        return value + " %";
                    },
                },
                data: response?.weightedCloudCoverDayAverage,
            },
            {
                name: "Energy average",
                type: "line",
                yAxisIndex: 1,
                tooltip: {
                    valueFormatter: function (value) {
                        return value + " kWh";
                    },
                },
                data: response?.energyAverage,
            },
        ],
    };

    return (
        <div>
        <button className='chart-page__button' onClick={handleShowClick} disabled={isLoading === true && response !== null}>Show</button>
            <ReactECharts
                option={options}
                notMerge={true}
                lazyUpdate={true}
                showLoading={isLoading === true || response === null}
            />
        </div>
    );
}

export default CloudCoverAverageAndCurrentPower;
