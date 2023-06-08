import { useState } from "react";
import { Card, CardBody, CardHeader, CardSubtitle, CardTitle, ListGroupItem, Spinner } from "reactstrap";
import { CiCircleInfo } from 'react-icons/ci';
import { TiWeatherSunny } from 'react-icons/ti';
import { FaCity, FaTemperatureHigh, FaTemperatureLow, FaTemperatureArrowUp, FaThermometerThreeQuarters, FaThermometerHalf } from "react-icons/fa";
import { FcCalendar } from 'react-icons/fc';
import { TbTemperatureMinus, TbTemperaturePlus } from 'react-icons/tb';
import { formatDate } from "../helpers";
import moment from "moment";
import { AreaChartWidget } from "./";
import _ from "lodash";

export function WeatherWidget({ displayState, weatherData, cardTitle, cardSubtitle }) {
    console.log("🚀 ~ file: WeatherWidget.jsx:7 ~ WeatherWidget ~ cardSubtitle:", cardSubtitle)
    console.log("🚀 ~ file: WeatherWidget.jsx:7 ~ WeatherWidget ~ cardTitle:", cardTitle)
    console.log("🚀 ~ file: WeatherWidget.jsx:7 ~ WeatherWidget ~ weatherData:", weatherData)
    console.log("🚀 ~ file: WeatherWidget.jsx:7 ~ WeatherWidget ~ displayState:", displayState)
    const { clickMessage, errorHeader, errorMessage, loadingMessage, weatherCardHeader, } = {
        clickMessage: "Click a brewery to see the weather",
        loadingMessage: "Loading weather. . .",
        weatherCardHeader: "Weather Forecast",
        errorHeader: 'An error has occurred',
        errorMessage: ''
    }

    const transformData = (data) => {

        return data;
    }



    switch (displayState) {
        default: return (
            <Card className="h-100 d-flex justify-content-center align-items-center">
                <div>
                    <div className="lead">{clickMessage}</div>
                    <div className="display-6 text-center"><CiCircleInfo /></div>
                </div>
            </Card>)
        case "loading": {
            return (
                <div className="h-100 d-flex justify-content-center align-items-center ">
                    <div>
                        <Spinner></Spinner>
                        <h6 className="lead">{loadingMessage}</h6>
                    </div>
                </div>
            )
        };
        case "hasLoaded": return (
            <Card className="h-100">
                <CardHeader>
                    <h3>{weatherCardHeader}<TiWeatherSunny /></h3>
                </CardHeader>
                <CardBody className="h-100 d-flex flex-column">
                    <div className="d-flex flex-row justify-content-between align-items-center">
                        <CardTitle><h4>{cardTitle}</h4></CardTitle>
                        <CardSubtitle><FaCity /><small>{cardSubtitle}</small></CardSubtitle>
                    </div>
                    <div>
                        <div className="list-group">
                            {
                                transformData(weatherData.temperaturesPerDays).map((record, idx) => {
                                    const min = _.maxBy(record.temperatures, o => o.airTemperature);
                                    const max = _.minBy(record.temperatures, o => o.airTemperature);
                                    return (
                                        <ListGroupItem key={idx} className="d-flex justify-content-between">
                                            <div style={{ flex: 2 }} >
                                                
                                                <h5><FcCalendar style={{fontSize: '1.3em'}} /><span className="ms-1">{formatDate(record.fullDate)}</span></h5>
                                                
                                                <hr className="my-1"></hr>
                                                <div className="text-center">
                                                {
                                                    record.averageTemperature < 25
                                                        ? <h2 className="text-primary display-6">
                                                            <FaThermometerHalf />{Math.floor(record.averageTemperature)}&deg;C
                                                        </h2>
                                                        : <h2 className="text-success display-6">
                                                            <FaThermometerThreeQuarters />{Math.floor(record.averageTemperature)}&deg;C
                                                        </h2>
                                                }
                                                    <small style={{color: "light-gray"}}>AVG Temp for the day</small>
                                                </div>
                                            </div>
                                            <div className="border-start ps-1" style={{ flex: 4 }}>
                                                <p>Air Temperature during. &deg;C/h</p>
                                                <div>
                                                    <AreaChartWidget dayTemperatures={record.temperatures} />
                                                </div>
                                            </div>
                                            <div style={{ flex: 1.3 }} className="border-start">
                                                <span className="ps-1">Min/Max:</span>
                                                <hr></hr>
                                                <h6 className="text-success ps-1"> {max.airTemperature}&deg;C<TbTemperaturePlus /></h6>
                                                <h6 className="text-primary ps-1"> {min.airTemperature}&deg;C<TbTemperatureMinus /></h6>
                                            </div>
                                        </ListGroupItem>
                                    )
                                })
                            }

                        </div>
                    </div>
                </CardBody>
            </Card>
        );
        case "error": return (
            <div>
                <h4>{errorHeader}</h4>
                <p className="lead">{errorMessage}</p>
            </div>
        )

    }
}