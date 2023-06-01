import { useParams } from "react-router-dom";

export function FriendlyError() {
    const { headingText, paragraph, image } = {
        headingText: 'Friendly Error',
        paragraph: 'Friendly Error Message',
        image: 'Friendly Error Image'
    }
    const params = useParams();

    console.log(params);

    return (
        <>
            <h1 className="display-4">{headingText}</h1>
            <hr />
            <p>{paragraph}</p>
            <p>{image}</p>
        </>
    )
}