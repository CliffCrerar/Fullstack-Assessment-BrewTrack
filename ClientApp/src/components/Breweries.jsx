import { Card, CardBody, ListGroup, ListGroupItem, Spinner } from "reactstrap";
import { FaCity, FaExternalLinkAlt, FaPhone } from 'react-icons/fa';
import { formatPhoneNumber, getFromApi } from "../helpers";
import { PaginationWidget } from "./PaginationWidget";
import { MdLocationOff } from 'react-icons/md'
import { useEffect, useRef, useState } from "react";
import { SiHomebrew } from 'react-icons/si'
import './Breweries.scss';
import _ from "lodash";
import { WeatherWidget } from "./WeatherWidget";

export function Breweries(props) {
	const userId = localStorage.getItem('userId');
	const weatherRef = useRef(null);
	const [pageState, setPageState] = useState({
		currentPage: null,
		pageData: null,
		totalPages: null,
		loaderStyle: { display: 'block' },
		weatherDisplay: { displayState: "", data: null, cardTitle: null, cardSubtitle: null },
	})
	const restApiEndpoint = '/api/Breweries/';
	const { current, prev, next, selected } = {
		meta: restApiEndpoint + 'page-number/' + userId,
		current: restApiEndpoint + 'current/' + userId,
		prev: restApiEndpoint + 'prev-page/' + userId,
		next: restApiEndpoint + 'next-page/' + userId,
		selected: restApiEndpoint + 'selected-page/' + userId
	}
	const showLoader = () => setPageState(currentState => ({ ...currentState, loaderStyle: { display: 'block' } }))
	const hideLoader = () => setPageState(currentState => ({ ...currentState, loaderStyle: { display: 'none' } }))
	const setWeatherDisplayState = (displayState, data = null) => setPageState(currentState => ({
		...currentState, weatherDisplay: {
			displayState,
			data,
			cardTitle: currentState.weatherDisplay.cardTitle,
			cardSubtitle: currentState.weatherDisplay.cardSubtitle
		}
	}))
	const setNameAndCity = (cardTitle, cardSubtitle) => setPageState(currentState => ({
		...currentState, weatherDisplay: {
			displayState: currentState.weatherDisplay.displayState,
			data: currentState.weatherDisplay.data,
			cardTitle,
			cardSubtitle
		}
	}))
	const handleResponse = (body) => {
		console.log(body);
		setPageState(prevState => {
			return {
				...prevState,
				currentPage: body.pageNo,
				pageData: body.pageData,
				totalPages: body.totalPages
			}
		})
		hideLoader();
	}
	const handleError = (error) => {
		// hideLoader();
	}
	const getCurrentPage = () => new Promise((resolve, reject) => {
		if (!_.isNil(pageState.pageData)) {
			return reject("No Page Data");
		}
		getFromApi(current)
			.then(response => {
				console.log(response);
				return response.json();
			})
			.then(resolve);
	})

	const getPrevPage = () => {
		showLoader();
		getFromApi(prev).then(handleResponse)
	}
	const getNextPage = () => {
		showLoader();
		getFromApi(next).then(handleResponse)
	}

	const getSelectedPage = (pageNo) => getFromApi(`${selected}/page-no/${pageNo}`).then(handleResponse)

	const getWeather = async (latitude, longitude, name, city) => {
		try {
			setNameAndCity(name, city);
			setWeatherDisplayState('loading');
			const getWeatherResponse = await getFromApi(`/api/weather?longitude=${longitude}&latitude=${latitude}`);
			if (!getWeatherResponse.ok) {
				const body = getWeatherResponse.text();
				const newError = new Error("Get weather error");
				newError.code = getWeatherResponse.code
				newError.stack = body;
				throw new Error("Get weather error")
			} else {
				const body = await getWeatherResponse.json();
				setWeatherDisplayState('hasLoaded', body);
				weatherRef.current.scrollIntoView();
			}
		} catch (error) {
			console.error(error.message, error.stack);
			setWeatherDisplayState('error', error)
		}
	}

	useEffect(() => {
		getCurrentPage().then(handleResponse).catch(handleError);
	})

	return (
		<>
			<div className="flex-1 ">
				<h3>Breweries</h3>
				{
					_.isNil(pageState.pageData)
						? <p>Loading breweries . . . .</p>
						: <>
							<hr />
							<Card className="py-2">
								<div className="container-fluid">
									<div className="row">
										<div className="col-md">
											<div style={pageState.loaderStyle} className="loading-overlay">
												<div className="spinner text-center">
													<Spinner color="white" size="large"></Spinner>
													<p className="lead text-light">Loading data...</p>
												</div>
											</div>
											<ListGroup className="infinite-scroll">
												{pageState.pageData.map(({ id, brewery_Type, city, latitude, longitude, name, phone, website_Url }, idx) => {
													return (
														<ListGroupItem key={idx} className="mw-100 col-md" aria-current="true">
															<div className="d-flex w-100 justify-content-between">
																<h5 className="mb-1">{name}</h5>
																<small className="flex-one text-end">
																	<a className="" hidden={website_Url == null} href={website_Url} target="_blank">See website  <FaExternalLinkAlt /></a>
																</small>
																<small hidden={website_Url != null} className="flex-one text-end opacity-50">
																	Website unavailable
																</small>
															</div>
															<div className="d-flex w-100 justify-content-between">
																<p className="flex-on lead m-0"><FaPhone /> {formatPhoneNumber(phone)}</p>
																<small hidden={latitude != null} >
																	<MdLocationOff className="mb-1" />No location info
																</small>
																<small hidden={latitude == null} className="flex-one text-end">
																	<button onClick={() => getWeather(latitude, longitude, name, city)} className="btn btn-outline-secondary btn-sm text-end">Weather info</button>
																</small>
															</div>
															<div className="d-flex w-100 justify-content-between">
																<small className="flex-one"><FaCity /> {city}</small>
																<div className="flex-one text-end"><SiHomebrew />: <strong> {brewery_Type}</strong></div>

															</div>
														</ListGroupItem>
													)
												})}
											</ListGroup>
											<div className="pt-2">
												<PaginationWidget
													pageCount={pageState.totalPages}
													currentPage={pageState.currentPage}
													onNextPage={getNextPage}
													onPrevPage={getPrevPage}
													onSelectedPage={getSelectedPage}>
												</PaginationWidget>
											</div>
										</div>
										<div ref={weatherRef} id="weatherWidget" className="col-md position-relative">
											<div className="infinite-scroll">
												<WeatherWidget
													displayState={pageState.weatherDisplay.displayState}
													weatherData={pageState.weatherDisplay.data}
													cardTitle={pageState.weatherDisplay.cardTitle}
													cardSubtitle={pageState.weatherDisplay.cardSubtitle}
												></WeatherWidget>
											</div>
										</div>

									</div>
								</div>
							</Card>

						</>
				}
			</div >
		</>
	)
}