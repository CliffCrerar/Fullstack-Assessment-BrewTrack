import { useState } from "react";
import { getFromApi, postToApi } from "../helpers/http-helper";
import { Button, Card, CardBody, CardHeader, CardSubtitle, Container, Input, InputGroup, InputGroupText, Label } from "reactstrap";
import {FcLock} from 'react-icons/fc'
import { useNavigate } from "react-router-dom";

export function Identity() {
    const [state, setState] = useState();
    const navigate = useNavigate();

    function submitEmailCheck(ev) {
        ev.persist();
        ev.preventDefault();
        checkEmail(formData.get('email'));
    }

    async function checkEmail(email) {
        const res = await fetch('api/User/Email/' + email);

        if(res.status === 200) {
            var body = await res.json();
            localStorage.setItem("userId", body.userId);
            navigate('/breweries')
        }

        if(res.status === 404) {
            navigate('/register?emailAddress=' + email)
        }

        const body = await res.text();
    }

    return (
        <>
            <Container>
                <Card color="light" style={{
                    maxWidth: '25rem'
                }} className="shadow m-auto">
                    <CardHeader className="bg-dark text-light">Provide us with your email</CardHeader>
                    <CardBody>
                        <form name="email-check-form" onSubmit={submitEmailCheck}>
                            <InputGroup >
                                <InputGroupText><FcLock/></InputGroupText>
                                <Input placeholder="Email Address"className="form-control" name="email" type="email" required></Input>
                            </InputGroup>
                            <CardSubtitle className="text-center p-3"><small>
                                We are only interested in your email address to identify you, no passwords welcome here, neither are we intending to send you emails in any way.
                            </small></CardSubtitle>
                            <hr />
                            <div>
                                <Button className="m-auto d-block" color="secondary">Submit</Button>
                            </div>
                        </form>
                        <hr />
                    </CardBody>
                </Card>
            </Container>
        </>
    )
}