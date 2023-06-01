import { Button, Card, CardBody, CardFooter, CardSubtitle, CardTitle, Input, InputGroup, InputGroupText, Spinner } from "reactstrap";
import { postToApi } from "../helpers";
import { useNavigate, useSearchParams } from "react-router-dom";
import React, { useEffect, useRef, useState } from "react";
import { FcAbout, FcAddressBook, FcCalendar } from "react-icons/fc";

export function Register(props) {
	const { formName, formHeader, formSubTitle, gotoIdentityButtonText } = {
		formName: 'newUserRegistrationForm',
		formHeader: 'New user registration',
		formSubTitle: 'To view the content of our site it is important for us to know who you are.',
		gotoIdentityButtonText: 'Try email again'
	}
	const [params] = useSearchParams();
	useEffect(() => { document.getElementById('emailAddressemail0').value = params.get('emailAddress'); });
	console.log("🚀 ~ file: Register.jsx:14 ~ Register ~ params:", params.get('emailAddress'))
	const [registrationStatus, setRegistrationStatus] = useState({ code: null, message: null, bg: null })
	const statusCodeMessages = [
		{
			code: 400,
			message: 'Something weird is going on right now!',
			bg: 'bg-danger'
		},
		{
			code: 409,
			message: 'User with this email address exists already, please try to identify yourself with the email you just attempted to register.',
			bg: 'bg-warning'
		},
		{
			code: 500,
			message: 'Wehhhhhhhhhheeeeeeeeeeeeeeeeeeeee!',
			bg: 'bg-danger'
		},
		{
			code: 201,
			message: 'Successfully registered',
			bg: 'bg-success'
		}
	]
	// Spinner state
	const
		[spinnerHidden, setSpinner] = useState(true),
		showSpinner = () => setSpinner(false),
		hideSpinner = () => setSpinner(true);
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

	// the form builder that consumes the above and turns it into jsx
	function FormBuilder({ paramSets }) {
		return paramSets.map(([type, name, text, required], idx) => {
			if (idx === paramSets.length - 1) {
				return <Button
					className="d-block m-auto mt-2"
					disabled={registrationStatus.code == 409}
					size="lg"
					key={idx}
					type={type}
					defaultValue={idx === 0 && params.get('emailAddress')}
					color={name}>
					{text}
					<Spinner hidden={spinnerHidden} size="sm"></Spinner>
				</Button>
			}
			return (
				<React.Fragment key={idx} >
					<InputGroup className="pt-3">
						<InputGroupText htmlFor={name + type + idx} >{icons[idx]}</InputGroupText>
						<Input
							disabled={registrationStatus.code == 409}
							id={name + type + idx}
							type={type}
							name={name}
							placeholder={text}
							required={!!required}
						/>
					</InputGroup>
					<small className="valid-feedback color-success">Validation</small>
					<small className="invalid-feedback color-danger">Validation</small>
				</React.Fragment>
			)
		})
	}

	// UX friendly navigator
	const
		navigate = useNavigate(),
		nav = (url) => setTimeout(() => {
			hideSpinner();
			navigate(url)
		}, 2000)

	// http call promise handlers
	const
		catchApiCallError = (error) => {
			console.error(error)
			nav('friendly-error?error' + error);
		},
		handlePostResponse = (response) => {
			console.log(response);
			var responseCode = response.status;
			setRegistrationStatus(statusCodeMessages.find(codeMessage => codeMessage.code === responseCode))
			switch (responseCode) {
				case 400: console.log('404'); break;
				case 500: nav('friendly-error?error=response')
				case 201: {
					response.json().then(data => {
						localStorage.setItem('userId', data.id)
						nav('/breweries')
					})
				}
			}
			hideSpinner();
		}

	// Form submission event handler
	function onRegistrationFormSubmit(ev) {
		showSpinner();
		ev.persist();
		ev.preventDefault();
		const
			formData = new FormData(ev.target),
			restEndpoint = '/api/user',
			body = {
				givenName: formData.get("givenName"),
				familyName: formData.get("familyName"),
				emailAddress: formData.get("emailAddress"),
				dateOfBirth: formData.get("dateOfBirth")
			}

		console.log(body);

		postToApi(
			restEndpoint,
			{
				givenName: formData.get("givenName"),
				familyName: formData.get("familyName"),
				emailAddress: formData.get("emailAddress"),
				dateOfBirth: formData.get("dateOfBirth")
			})
			.catch(catchApiCallError)
			.then(handlePostResponse)
	}

	// card styles 
	const cardStyles = {
		maxWidth: '24rem'
	}

	const onGotoEnterEmail = () => {
		showSpinner()
		nav('/login')

	}

	return (
		<Card style={cardStyles} color="light" className="shadow">

			<CardBody>
				<CardTitle>
					<h2>{formHeader}</h2>
				</CardTitle>
				<CardSubtitle>{formSubTitle}</CardSubtitle>
				<hr />
				<form className="needs-validation" onSubmit={onRegistrationFormSubmit} name={formName}>
					<FormBuilder paramSets={formMetaData} />
				</form>
			</CardBody>

			<CardFooter hidden={!Boolean(registrationStatus.code)} className={registrationStatus.bg} color="danger">
				<p className="lead">{registrationStatus.message}</p>
				<Button className="d-block" color="danger" hidden={registrationStatus.code != 409} size="sm" onClick={onGotoEnterEmail}>
					{gotoIdentityButtonText}<Spinner size="sm" hidden={spinnerHidden}></Spinner>
				</Button>
			</CardFooter>
		</Card>
	)
}