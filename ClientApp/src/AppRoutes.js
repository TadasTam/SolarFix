import ApiAuthorzationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import UpdateDatabase from "./components/UpdateDatabase";
import { Home } from "./components/Home";
import InverterDataTable from "./components/InverterDataTable";
import MonitoringPage from "components/MonitoringPage";
import React from "react";
import CloudCoverAndCurrentPowerChartPage from "components/CloudCoverAndCurrentPowerChartPage";
import CloudCoverAndCurrentPowerInvertersChartPage from "components/CloudCoverAndCurrentPowerInvertersChartPage";
import CloudCoverAverageAndCurrentPowerPage from "components/CloudCoverAverageAndCurrentPowerPage";
import DayProducedEnergyChartPage from "components/DayProducedEnergyChartPage";

const AppRoutes = [
    {
        index: true,
        element: <Home />,
    },
    {
        path: "/inverterdatatable",
        element: <InverterDataTable />,
    },
    {
        path: "/updatedatabase",
        element: <UpdateDatabase />,
    },
    {
        path: "/monitoring",
        element: <MonitoringPage />,
    },
    {
        path: "/CloudCoverAndCurrentPowerChartPage",
        element: <CloudCoverAndCurrentPowerChartPage />,
    },
    {
        path: "/CloudCoverAndCurrentPowerInvertersChartPage",
        element: <CloudCoverAndCurrentPowerInvertersChartPage />,
    },
    {
        path: "/CloudCoverAverageAndCurrentPowerPage",
        element: <CloudCoverAverageAndCurrentPowerPage />,
    },
    {
        path: "/DayProducedEnergyChartPage",
        element: <DayProducedEnergyChartPage />,
    },
    ...ApiAuthorzationRoutes,
];

export default AppRoutes;
