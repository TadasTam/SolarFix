import { useState, useMemo } from "react";
import authService from "./api-authorization/AuthorizeService";
import MaterialReactTable from "material-react-table";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import React from "react";
import { useLocation } from "react-router-dom";

// @ts-ignore
function InverterDataTable(props) {
    var [startDate, setStartDate] = useState(new Date());
    var [endDate, setEndDate] = useState(new Date());
    const [data, setData] = useState([]);
    const [Loading, setLoading] = useState(false);

    const handleStartDateChange = (date) => {
        setStartDate(date);
    };

    const handleEndDateChange = (date) => {
        setEndDate(date);
    };

    const handleSearchClick = () => {
        setLoading(true);
        authService.getAccessToken().then((token) => {
            fetch(
                `databasetofront/GetDataByDate?startDate=${encodeURIComponent(
                    startDate.toISOString()
                )}&endDate=${encodeURIComponent(endDate.toISOString())}`,
                {
                    headers: !token ? {} : { Authorization: `Bearer ${token}` },
                }
            ).then((response) => {
                response.json().then((data) => {
                    console.log(data);
                    setData(data);
                    setLoading(false);
                });
            });
        });
    };
    
    const location = useLocation();
    // @ts-ignore
    if (location.state?.startDate && location.state?.endDate)
    {
        // @ts-ignore
        setStartDate(location.state.startDate);
        // @ts-ignore
        setEndDate(location.state.endDate);

        // @ts-ignore
        startDate = location.state.startDate;
        // @ts-ignore
        endDate = location.state.endDate;

        // @ts-ignore
        location.state.startDate = null;
        // @ts-ignore
        location.state.endDate = null;

        handleSearchClick();
    }

    const columns = useMemo(
        () => [
            {
                accessorKey: "inverterId",
                header: "Inverter Id",
            },
            {
                accessorKey: "date",
                header: "Date",
            },
            {
                accessorKey: "currentPower",
                header: "Current power",
            },
            {
                accessorKey: "dailyProducedEnergy",
                header: "Daily produced energy",
            },
            {
                accessorKey: "temperature",
                header: "Inverter temperature",
            },
            {
                accessorKey: "pV1Voltage",
                header: "PV1 Voltage",
            },
            {
                accessorKey: "pV2Voltage",
                header: "PV2 Voltage",
            },
            {
                accessorKey: "totalEnergy",
                header: "Total energy",
            },
            {
                accessorKey: "pV1InputPower",
                header: "PV1 Input power",
            },
            {
                accessorKey: "pV2InputPower",
                header: "PV2 Input power",
            },
            {
                accessorKey: "heatsinkTemperature",
                header: "Heatsink temperature",
            },
            {
                accessorKey: "currentGridR",
                header: "Current grid R",
            },
            {
                accessorKey: "voltageGridR",
                header: "Voltage grid R",
            },
            {
                accessorKey: "frequencyGridR",
                header: "Frequncy grid R",
            },
            {
                accessorKey: "currentGridS",
                header: "Current grid S",
            },
            {
                accessorKey: "voltageGridS",
                header: "Voltage grid S",
            },
            {
                accessorKey: "frequencyGridS",
                header: "Frequncy grid S",
            },
            {
                accessorKey: "currentGridT",
                header: "Current grid T",
            },
            {
                accessorKey: "voltageGridT",
                header: "Voltage grid T",
            },
            {
                accessorKey: "frequencyGridT",
                header: "Frequncy grid T",
            },
        ],
        []
    );


    const filterStart = (date) => date <= new Date() && date <= endDate;
    const filterEnd = (date) => date <= new Date() && date > addDays(startDate, -1);
    function addDays(date, days) { return (new Date(date)).setDate(date.getDate() + days) }

    return (
        <div className="chart-page">
            <div className="chart-page__header">
                <label className="chart-page__label">Start Date: </label>
                <DatePicker
                    selected={startDate}
                    onChange={handleStartDateChange}
                    showMonthDropdown
                    showYearDropdown
                    filterDate={filterStart}
                    className="chart-page__datepicker"
                />
                <label className="chart-page__label">End Date: </label>
                <DatePicker
                    selected={endDate}
                    onChange={handleEndDateChange}
                    showMonthDropdown
                    showYearDropdown
                    filterDate={filterEnd}
                    className="chart-page__datepicker"
                />

            </div>

            <div className="chart-page__chart">
                <button className='chart-page__button' onClick={handleSearchClick} disabled={Loading}>Search</button>
                <MaterialReactTable
                    columns={columns}
                    data={data}
                    state={{ isLoading: Loading }}
                />
            </div>
        </div>
    );
}

export default InverterDataTable;
