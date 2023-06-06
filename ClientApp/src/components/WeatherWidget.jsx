import { Card, CardBody, CardHeader, CardSubtitle, CardTitle } from "reactstrap";

export function WeatherWidget(props) {

    return (
        <>
            <Card className="h-100">
                <CardHeader>
                    <h3>Card Header</h3>
                </CardHeader>
                <CardBody>
                    <CardTitle>Card Body</CardTitle>
                    <CardSubtitle>Card SubTitle</CardSubtitle>
                </CardBody>
            </Card>
        </>
    )

}