import { ListGroup, ListGroupItem } from "reactstrap";
import { Pagination } from "./Pagination";
import './Breweries.scss';
import { getFromApi } from "../helpers";
import { useState } from "react";
import _ from "lodash";
export function Breweries(props) {
	const userId = sessionStorage.getItem('userId');
	const [pageState, setPageState] = useState({
		
		currentPage: null,
		pageData: null,
		totalPages: null,
	})
	const restApiEndpoint = '/api/Breweries/'
	const {current, prev, next} = {
		meta: restApiEndpoint + 'page-number/' + userId,
		current: restApiEndpoint + 'current/' + userId,
		prev: restApiEndpoint + 'prev-page/' + userId,
		next:restApiEndpoint+  'next-page/' + userId
	}
	const getCurrentPage = () => new Promise((resolve,reject)=>{
		if(!_.isNil(pageState.pageData)) {
			return reject();
		}
		getFromApi(current)
		.then(response => {
			console.log(response);
			return response.json();
		})
		.then(resolve);
	})
		
	const getPrevPage = () => getFromApi(prev)
	const getNextPage = () => getFromApi(next)


	getCurrentPage()
	.then(({lastVisitedPage,totalPages,data}) => {
		console.log(lastVisitedPage,totalPages,data);
		setPageState(prevState => {
			return {
				...prevState,
				currentPage: lastVisitedPage === 0 ? 1 : lastVisitedPage,
				pageData: data,
				totalPages				
			}
		})
	})

	return (
		<>
			<div className="flex-1 ">
				<h3>Breweries</h3>
				{
					_.isNil(pageState.pageData) 
					? <p>Loading breweries . . . .</p>
					: <>
						<hr/>
							<div className="infinite-scroll">
								<ListGroup>
									{pageState.pageData.map((entry,idx) => {
										console.log(entry);

										return (
											<ListGroupItem key={idx} className="list-group-item-action" aria-current="true">
											<div className="d-flex w-100 justify-content-between">
												<h5 className="mb-1">List group item heading</h5>
												<small>3 days ago</small>
											</div>
											<p className="mb-1">Some placeholder content in a paragraph.</p>
											<small>And some small print.</small>
				
										</ListGroupItem>
										)
									})}
								</ListGroup>
							</div>
							<div className="pt-3">
								<Pagination  nextPage={getNextPage} getPrevPage={getPrevPage}></Pagination>
							</div>
							</>
				}
				

			</div>
		</>
	)
}