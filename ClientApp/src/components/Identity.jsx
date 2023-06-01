import { Button, Card, CardBody, CardHeader, CardSubtitle, Container, Input, InputGroup, InputGroupText, Label } from "reactstrap";
import { FcLock } from 'react-icons/fc'
import { useNavigate } from "react-router-dom";
import { getFromApi, postToApi } from "../helpers/http-helper";

/**
 * @description Component used to identify users by their email address
 */
export function Identity() {

    // display text object
    const componentText = {
        cardHeader: 'Provide us with your email',
        cardSubtitle: 'We are only interested in your email address to identify you, no passwords welcome here, neither are we intending to send you emails in any way.'
    }

    const navigate = useNavigate(); // navigation function

    // form submission handler
    function onCheckEmailSubmit(ev) {
        ev.persist();
        ev.preventDefault();
        const formData = new FormData(ev.target);
        checkEmail(formData.get('email'));
    }

    // check email rest api handler
    async function checkEmail(email) {
        const res = await getFromApi('api/User/Email/' + email);

        if (res.status === 200) {
            const body = await res.json();
            localStorage.setItem("userId", body.userId);
            navigate('/breweries')
        }

        if (res.status === 404) {
            navigate('/register?emailAddress=' + email)
        }
    }

    return (
        <>
            <Container>
                <Card color="light" style={{
                    maxWidth: '25rem'
                }} className="shadow m-auto">
                    <CardHeader className="bg-dark text-light">{componentText.cardHeader}</CardHeader>
                    <CardBody>
                        <form name="email-check-form" onSubmit={onCheckEmailSubmit}>
                            <InputGroup >
                                <InputGroupText><FcLock /></InputGroupText>
                                <Input placeholder="Email Address" className="form-control" name="email" type="email" required></Input>
                            </InputGroup>
                            <CardSubtitle className="text-center p-3">
                                <small>
                                    {componentText.cardSubtitle}
                                </small>
                            </CardSubtitle>
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