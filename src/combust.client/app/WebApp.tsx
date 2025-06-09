import {
    NavDrawerProps,
} from "@fluentui/react-nav-preview";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import DashboardLayout from "./layouts/DashboardLayout";
//import RealWeatherForecast from "./pages/RealWeatherForecast";

export const WebApp = (props: Partial<NavDrawerProps>) => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<DashboardLayout />} />
            </Routes>
        </BrowserRouter>
    );
};

export default WebApp;