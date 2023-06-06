import { Card, ListGroup, ListGroupItem, Spinner } from "reactstrap";
import { FaCity, FaExternalLinkAlt, FaPhone } from 'react-icons/fa';
import { formatPhoneNumber, getFromApi } from "../helpers";
import { PaginationWidget } from "./PaginationWidget";
import { MdLocationOff } from 'react-icons/md'
import { useEffect, useState } from "react";
import { SiHomebrew } from 'react-icons/si'
import './Breweries.scss';
import _ from "lodash";
import { WeatherWidget } from "./WeatherWidget";

export function Breweries(props) {
	const userId = localStorage.getItem('userId');
	const [pageState, setPageState] = useState({
		currentPage: null,
		pageData: null,
		totalPages: null,
		loaderStyle: { display: 'block' }
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

	const getWeather = (longitude, latitude) => {
		getFromApi(`/api/weather?longitude=${longitude}&latitude=${latitude}`);
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
							<div className="container-fluid">
								<div className="row">
									<div className="col-md">
										<div className="infinite-scroll position-relative">
											<div style={pageState.loaderStyle} className="loading-overlay">
												<div className="spinner text-center">
													<Spinner color="white" size="large"></Spinner>
													<p className="lead text-light">Loading data...</p>
												</div>
											</div>
											<ListGroup>

												{pageState.pageData.map(({ id, brewery_Type, city, latitude, longitude, name, phone, website_Url }, idx) => {
													return (
														<ListGroupItem key={idx} className="border-start mw-100 col-md " aria-current="true">
															<div className="d-flex w-100 justify-content-between">
																<h5 className="mb-1">{name}</h5>

																<small className="flex-one text-end">
																	<a className="" href={website_Url} target="_blank">See website  <FaExternalLinkAlt /></a>
																</small>
															</div>
															<div className="d-flex w-100 justify-content-between">
																<p className="mb-1 flex-one"><FaCity />: {city}</p>

																{/*<div className="flex-one text-center" hidden={longitude == null}>
																<button className="btn-primary btn btn-sm" href={locationLink(longitude, latitude)} hidden={longitude == null} target="_blank">
																	<MdLocationOn className="mb-1" />
																	View location
																</button>
															</div> */}
																<small hidden={latitude != null} >
																	<MdLocationOff className="mb-1" />No location info
																</small>
																<small hidden={latitude == null} className="flex-one text-end">
																	<button onClick={() => getWeather(longitude, latitude)} className="btn btn-secondary btn-sm text-end">Weather info</button>
																</small>
															</div>
															<div className="d-flex w-100 justify-content-between">
																<small className="flex-one"><FaPhone /> {formatPhoneNumber(phone)}</small>
																<div className="flex-one text-end"><SiHomebrew />: <strong> {brewery_Type}</strong></div>

															</div>
														</ListGroupItem>
													)
												})}
											</ListGroup>
										</div>
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
									<div className="col-md position-relative">
										<div className="h-100">
											<WeatherWidget></WeatherWidget>
										</div>
									</div>

								</div>
							</div>


						</>
				}


			</div >
		</>
	)
}