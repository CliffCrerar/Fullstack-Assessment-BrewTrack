import { Button, Card, CardBody, CardHeader, CardSubtitle, Container, Input, InputGroup, InputGroupText, Label, Spinner } from "reactstrap";
import { FcLock } from 'react-icons/fc'
import { useNavigate } from "react-router-dom";
import { getFromApi } from "../helpers";
import { useState } from "react";

/**
 * @description Component used to identify users by their email address
 */
export function Identity() {

    const // dev values
        testVal = {
            useDevValue: false,
            value: 'Cliff.Crerar@gmail.com'
        };

    const cardStyles = { // card styles
        maxWidth: '25rem'
    }

    const [formData, setFormData] = useState({
        email: ""
    })

    const handleChange = (changeEvent) =>  {
        const {name, value} = changeEvent.target;
        setFormData(prevData => ({...prevData, email: value}) )
    }

    const // spinner states handlers
        [spinnerHidden, setSpinner] = useState(true),
        showSpinner = () => setSpinner(false), // show spinner 
        hideSpinner = () => setSpinner(true); // hide spinner

    // display text object
    const { cardHeader, cardSubtitle, buttonText } = {
        cardHeader: 'Provide us with your email',
        cardSubtitle: 'We are only interested in your email address to identify you, no passwords welcome here, neither are we intending to send you emails in any way.',
        buttonText: 'Submit'
    }

    const navigate = useNavigate(); // navigation function

    const nav = (url) => {
        setTimeout(() => {
            hideSpinner();
            navigate(url)
        }, 1000)
    }

    // form submission handler
    function onCheckEmailSubmit(ev) {
        ev.preventDefault();
        ev.persist();
        showSpinner();
        checkEmail(formData.email)
            .catch(error => {
                console.error(error);
                nav('/friendly-error?error=' + error)
            })
            .then(value => {
                const navTo = value
                    ? '/breweries'
                    : '/register?emailAddress=' + formData.email;
                nav(navTo);
            });
    }

    // check email rest api handler
    async function checkEmail(email) {
        const res = await getFromApi('api/User/Email/' + email);
        if (res.status === 200) {
            const body = await res.json();
            console.log("🚀 ~ file: Identity.jsx:70 ~ checkEmail ~ body:", body)
            sessionStorage.setItem("userId", body.id);
            return true;
        } else if (res.status === 404) {
            return false;
        } else {
            throw res;
        }
    }

    return (
        <Container>

            <Card color="light" style={cardStyles} className="shadow m-auto">

                <CardHeader className="bg-dark text-light">{cardHeader}</CardHeader>

                <CardBody>

                    <form name="email-check-form" onSubmit={(ev)=>onCheckEmailSubmit(ev)}>

                        <InputGroup >
                            <InputGroupText><FcLock /></InputGroupText>
                            <Input
                                onChange={(c) =>  handleChange(c) }
                                placeholder="Email Address"
                                className="form-control"
                                name="email"
                                type="email"
                                required
                                defaultValue={testVal.useDevValue ? testVal.value : ''} />
                        </InputGroup>

                        <CardSubtitle className="text-center p-3">
                            <small>
                                {cardSubtitle}
                            </small>
                        </CardSubtitle>
                        <hr />
                        <Button type="submit" className="m-auto d-block" color="secondary">
                            {buttonText}<Spinner hidden={spinnerHidden} size="sm"></Spinner>
                        </Button>
                        <hr />

                    </form>

                </CardBody>

            </Card>

        </Container>

    )
}