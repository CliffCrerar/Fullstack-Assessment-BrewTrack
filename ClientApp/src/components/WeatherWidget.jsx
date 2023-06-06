import { useState } from "react";
import { Card, CardBody, CardHeader, CardSubtitle, CardTitle, ListGroupItem, Spinner } from "reactstrap";
import { CiCircleInfo } from 'react-icons/ci';
import { TiWeatherSunny } from 'react-icons/ti';

export function WeatherWidget({ displayState, weatherData }) {




    switch (displayState) {
        default: return (
            <Card className="h-100 d-flex justify-content-center align-items-center">
                <div>
                    <div className="lead">Click a brewery to see the weather</div>
                    <div className="display-6 text-center"><CiCircleInfo /></div>
                </div>
            </Card>)
        case "loading": {
            return <div><Spinner></Spinner></div>
        };
        case "hasLoaded": return (
            <>
                <Card className="h-100">
                    <CardHeader>
                        <h3>Weather Forecast<TiWeatherSunny /></h3>
                    </CardHeader>
                    <CardBody className="h-100">

                        <CardTitle><h4>Card Body</h4></CardTitle>
                        <CardSubtitle>Card SubTitle</CardSubtitle>
                        <div style={{ overflow: 'scroll', heigth: '100%' }}>
                            <div className="list-group">
                                {
                                    weatherData.hours.map(record =>
                                        <ListGroupItem className="d-flex justify-content-between">
                                            <span>{record.time.replace(/ /g, "")}</span>  <div className="flex-end">{record.airTemperature.noaa} degrees C</div>
                                        </ListGroupItem>)
                                }

                            </div>
                        </div>
                    </CardBody>
                </Card>
            </>
        );
        case "error": return <div>
            <h4>An Error has occurred.</h4>
            <p className="lead"></p>
        </div>

    }
}