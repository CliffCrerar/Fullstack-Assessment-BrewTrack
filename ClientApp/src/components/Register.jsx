import { Button, Card, CardBody, CardFooter, CardSubtitle, CardTitle, Input, InputGroup, InputGroupText, Spinner } from "reactstrap";
import { postToApi } from "../helpers";
import { useNavigate, useSearchParams } from "react-router-dom";
import React, { useEffect, useRef, useState } from "react";
import { FcAbout, FcAddressBook, FcCalendar } from "react-icons/fc";
import _ from "lodash";
import { FormBuilder } from "./Formbuilder";
import { getStatusMessage } from "../static";

const formInitValues = {givenName:'',familyName:'',emailAddress:'',dateOfBirth:''}
const restEndpoint = '/api/user';

export function Register(props) {
	const { formName, formHeader, formSubTitle, gotoIdentityButtonText } = {
		formName: 'newUserRegistrationForm',
		formHeader: 'New user registration',
		formSubTitle: 'To view the content of our site it is important for us to know who you are.',
		gotoIdentityButtonText: 'Back Home'
	}
	
	const [params] = useSearchParams();
	const [registrationStatus, setRegistrationStatus] = useState({ code: null, message: null, bg: null, formSent: false });
	// Spinner state
	const
		[spinnerHidden, setSpinner] = useState(true),
		showSpinner = () => setSpinner(false),
		hideSpinner = () => setSpinner(true);
	// UX friendly navigator
	const
		navigate = useNavigate(),
		nav = (url) => setTimeout(() => {
			hideSpinner();
			navigate(url)
		}, 2000)

	useEffect(()=>{
		_.isEmpty(formData.emailAddress) && setFormData(prevFormData => ({...prevFormData, emailAddress: params.get('emailAddress')}))
	})

	// serialized as string meta data for registration form, 3
	// mapping is > array of { input type, input name attribute, display text/label/placeholder, required, validation message } 
	// the last set is for the submit button and is mapped array of {button type, color, button text}
	const
		icons = [<FcAddressBook />, <FcAbout />, <FcAbout />, <FcCalendar />],
		formMetaData = [
			'email,emailAddress,Email,true',
			'text,givenName,First Name,true',
			'text,familyName,Last Name,true',
			'date,dateOfBirth,Date of Birth,true',
			'submit,secondary,Sign Up'
		].map(row => row.split(','))

	// http call promise handlers
	const
		catchApiCallError = (error) => {
			console.error(error)
			nav('friendly-error?error' + error);
		},
		handlePostResponse = (response) => {
			console.log(response);
			const statusMessage = getStatusMessage(response.status);
			switch (response.status) {
				case 400: console.log('404'); break;
				case 500: nav('friendly-error?error=response'); break;
				case 201: {
					response.json().then(data => {
						localStorage.setItem('userId', data.id)
						nav('/breweries')
					});
					break;
				}
			}
			setRegistrationStatus(statusMessage);
			hideSpinner();
		}

	// form state
	const 
		[formData, setFormData] = useState(formInitValues),
		handleChange = (changeEvent) => {
			const {name, value} = changeEvent.target;
			setFormData(prevFormData => ({...prevFormData, [name]: value}));
		} 

	// Form submission event handler
	function onRegistrationFormSubmit(ev) {
		ev.preventDefault();
		showSpinner();
		postToApi(restEndpoint,formData)
			.catch(catchApiCallError)
			.then(handlePostResponse)
	}

	// card styles 
	const cardStyles = {
		maxWidth: '24rem'
	}

	const onGotoEnterEmail = () => {
		showSpinner()
		nav('/')
	}

	return (
		<Card style={cardStyles} color="light" className="shadow">

			<CardBody>
				<CardTitle>
					<h2>{formHeader}</h2>
				</CardTitle>
				<CardSubtitle>{formSubTitle}</CardSubtitle>
				<hr />
				<form onSubmit={onRegistrationFormSubmit} name={formName}>
					<FormBuilder 
					formState={formData} 
					onChange={handleChange} 
					paramSets={formMetaData} 
					spinnerState={spinnerHidden || [409,400,500].includes(registrationStatus.code) }
					buttonDisabled={ registrationStatus.code != null }
					icons={icons} />
				</form>
			</CardBody>

			<CardFooter hidden={!Boolean(registrationStatus.code)} className={registrationStatus.bg}>
				<p className="lead">{registrationStatus.message} <Spinner size="sm" hidden={registrationStatus.code!==201} ></Spinner></p>
				<Button className={registrationStatus.code==201 ? 'invisible' : 'd-block m-auto'} color="danger"  size="sm" onClick={onGotoEnterEmail}>
					{gotoIdentityButtonText}<Spinner size="sm" hidden={spinnerHidden}></Spinner>
				</Button>
			</CardFooter>
		</Card>
	)
}