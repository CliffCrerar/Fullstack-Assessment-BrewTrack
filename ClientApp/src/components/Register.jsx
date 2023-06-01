import { Card, CardBody, CardSubtitle, CardTitle, Input, InputGroup, InputGroupText } from "reactstrap";
import { postToApi } from "../helpers/http-helper";
import { useNavigate } from "react-router-dom";

export function Register(props) {

	const navigate = useNavigate();

	function onRegistrationFormSubmit(ev) {
		ev.persist();
		ev.preventDefault();
		const formData = new FormData(ev.target);
		postToUserApi({
			givenName: formData.get("givenName"),
			familyName: formData.get("familyName"),
			emailAddress: formData.get("emailAddress"),
			dateOfBirth: formData.get("dateOfBirth")
		})
			.catch(error => {
				console.error(error);
			})
			.then(result => {
				console.log(result);
				// post to localstore
				// navigate to breweries
			})
	}

	async function postToUserApi(postBody) {
		var res = await postToApi("/api/User", postBody);
		console.log("🚀 ~ file: Register.js:25 ~ postToUserApi ~ res:", res)
		return res;
	}

	return (
		<>
			<Card>
				<CardBody>
					<CardTitle>Register</CardTitle>
					<CardSubtitle>New User</CardSubtitle>
					<form onSubmit={onRegistrationFormSubmit} name="register user">
						<InputGroup>
							<InputGroupText>Email</InputGroupText>
							<Input type="email" required name="emailAddress" placeHolder="Email Address"></Input>
						</InputGroup>
						<InputGroup>
							<InputGroupText>First name</InputGroupText>
							<Input type="text" required name="givenName" placeHolder="First Name"></Input>
						</InputGroup>
						<InputGroup>
							<InputGroupText>Last name</InputGroupText>
							<Input type="text" required name="familyName" placeHolder="Last Name"></Input>
						</InputGroup>
						<InputGroup>
							<InputGroupText>Last name</InputGroupText>
							<Input type="date" required name="dateOfBirth"></Input>
						</InputGroup>
					</form>
				</CardBody>
			</Card>
		</>
	)
}