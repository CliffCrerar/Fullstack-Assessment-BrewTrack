import { Card, CardBody, CardSubtitle, InputGroupText } from "reactstrap";
import { postToApi } from "../helpers/http-helper";


export function Register() {

    function onRegistrationFormSubmit(ev) {
        ev.persist();
        ev.preventDefault();
        var formData = new FormData(ev.target);
        var postBody = {
            givenName: formData.get("givenName"),
            familyName: formData.get("familyName"),
            emailAddress: formData.get("emailAddress"),
            dateOfBirth: formData.get("dateOfBirth"),

        }
    }

    async function postToUserApi(postBody) {
        
        var res =  await postToApi("/api/User", postBody);
        console.log("🚀 ~ file: Register.js:25 ~ postToUserApi ~ res:", res)
    }

    return (
        <>
            <Card>
                <CardBody>
                    <CardTitle>Register</CardTitle>
                    <CardSubtitle>New User</CardSubtitle>

                    <form onSubmit={onRegistrationFormSubmit}  name="register user">
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