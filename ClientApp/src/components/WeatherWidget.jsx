import { useState } from "react";
import { Card, CardBody, CardHeader, CardSubtitle, CardTitle, ListGroupItem, Spinner } from "reactstrap";
import { CiCircleInfo } from 'react-icons/ci';
import { TiWeatherSunny } from 'react-icons/ti';

export function WeatherWidget({ displayState, weatherData }) {
    const { clickMessage, errorHeader, errorMessage, loadingMessage, weatherCardHeader, weatherCardSubTitle, weatherCardTitle } = {
        clickMessage: "Click a brewery to see the weather",
        loadingMessage: "Loading weather. . .",
        weatherCardHeader: "Weather Forecast",
        weatherCardTitle: 'Card Body',
        weatherCardSubTitle: 'Card SubTitle',
        errorHeader: 'An error has occurred',
        errorMessage: ''
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

                    <CardTitle><h4>{weatherCardTitle}</h4></CardTitle>
                    <CardSubtitle>{weatherCardSubTitle}</CardSubtitle>

                    <div style={{ overflow: 'scroll', heigth: '100%', flex: 1 }}>
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
        );
        case "error": return (
            <div>
                <h4>{errorHeader}</h4>
                <p className="lead">{errorMessage}</p>
            </div>
        )

    }
}