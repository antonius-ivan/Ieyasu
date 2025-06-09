import { Radio, RadioGroup, Switch, Table, TableBody, TableCell, TableHeader, TableHeaderCell, TableRow, useId } from "@fluentui/react-components";
import React, { useEffect, useState } from "react";

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

const RealWeatherForecast = () => {
    const radioGroupId = useId("rtype");
    const [forecasts, setForecasts] = useState<Forecast[]>();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents =
        forecasts === undefined ? (
            <p>
                <em>
                    Loading... Please refresh once the ASP.NET backend has started. See{" "}
                    <a href="https://aka.ms/jspsintegrationreact">
                        https://aka.ms/jspsintegrationreact
                    </a>{" "}
                    for more details.
                </em>
            </p>
        ) : (
            <Table aria-labelledby="tableLabel">
                <TableHeader>
                        <TableRow>
                            <TableHeaderCell>Date</TableHeaderCell>
                            <TableHeaderCell>Temp. (C)</TableHeaderCell>
                            <TableHeaderCell>Temp. (F)</TableHeaderCell>
                            <TableHeaderCell>Summary</TableHeaderCell>
                    </TableRow>
                </TableHeader>
                <TableBody>
                        {forecasts.map((forecast) => (
                            <TableRow key={forecast.date}>
                                <TableCell>{forecast.date}</TableCell>
                                <TableCell>{forecast.temperatureC}</TableCell>
                                <TableCell>{forecast.temperatureF}</TableCell>
                                <TableCell>{forecast.summary}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        );

    async function populateWeatherData() {
        const response = await fetch("weatherforecast");
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }

    return (
        <div>
            <h1 id="tableLabel">Weather Forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
};

export default RealWeatherForecast;
