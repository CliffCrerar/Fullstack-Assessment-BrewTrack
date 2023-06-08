import { useState } from "react";
import { Card, CardBody, CardHeader, CardSubtitle, CardText, CardTitle, ListGroupItem, Spinner } from "reactstrap";
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
    const { clickMessage, errorHeader, errorMessage, loadingMessage, weatherCardHeader,errorAlt } = {
        clickMessage: "Click a brewery to see the weather",
        loadingMessage: "Loading weather. . .",
        weatherCardHeader: "Weather Forecast",
        errorHeader: 'You have hit a twoddle.',
        errorMessage: 'Because twoddle is in effect, there are some guys with glasses scrambling about loosing their minds right now. \n\n Let us hope that sanity with functionality has returned by tomorrow.',
        errorAlt: 'Unfortunatly you will find no support for your troubles here, please try elsewhere. Usually we will at this point apologize, and we do, but we dont really care about you we just provide you with some form and silly email to provide the illusion that we care but we dont.'
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
                                        <ListGroupItem key={idx}>
                                            <div className="container-fluid">
                                                <div className="row">

                                                    <div className="col-md-12 col-lg-3" >
                                                        <h5><FcCalendar style={{ fontSize: '1.3em' }} /><span className="ms-1">{formatDate(record.fullDate)}</span></h5>
                                                        <hr className="my-1"></hr>
                                                        <div className="text-center">
                                                            {
                                                                record.averageTemperature < 25
                                                                    ? <h2 className="text-primary">
                                                                        <FaThermometerHalf style={{fontSize: '0.85em'}} />{Math.floor(record.averageTemperature)}&deg;C
                                                                    </h2>
                                                                    : <h2 className="text-success">
                                                                        <FaThermometerThreeQuarters style={{fontSize: '0.85em'}} />{Math.floor(record.averageTemperature)}&deg;C
                                                                    </h2>
                                                            }
                                                            <small style={{ color: "light-gray" }}>Average</small>
                                                        </div>
                                                    </div>
                                                    <div className="border-start ps-1 col-lg-7 col-md-12" >
                                                        <p>Air Temperature during. &deg;C/h</p>
                                                        <div>
                                                            <AreaChartWidget dayTemperatures={record.temperatures} />
                                                        </div>
                                                    </div>
                                                    <div className="border-start col-lg-2 col-md-12">
                                                        <span className="ps-1">Min/Max:</span>
                                                        <hr></hr>
                                                        <p className="text-success ps-1"> {max.airTemperature}&deg;C<TbTemperaturePlus /></p>
                                                        <p className="text-primary ps-1"> {min.airTemperature}&deg;C<TbTemperatureMinus /></p>
                                                    </div>
                                                </div>
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
            <Card className="card mb-3 w-75 m-auto">

                <CardHeader className="bg-warning text-danger">
                    <h4>Twoddle!!!</h4>
                </CardHeader>
                <div className="card-img-top bg-white text-center">
                    <img  src="img/error.gif" alt="Card image cap"></img>
                </div>
                <hr/>
                <CardBody>
                    <CardTitle>
                    <h4>{errorHeader}</h4>
                    </CardTitle>
                    <CardSubtitle>
                    <p className="lead">
                    TwoddleType: <strong>{weatherData.message}</strong>
                        </p>
                    </CardSubtitle>
                    <CardText>
                    
                    <hr/>
                    {errorMessage}
                    <hr/>
                    <small style={{fontSize: '0.6rem'}}>{errorAlt}</small>
                    </CardText>
                </CardBody>
                
                
            </Card>
        )

    }
}