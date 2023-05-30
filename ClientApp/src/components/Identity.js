import { useState } from "react";
import { getFromApi, postToApi } from "../helpers/http-helper";


export function Identity() {
    const [state, setState]  = useState();

    function submitEmailCheck(ev) {
        console.log(ev);
        ev.persist();
        ev.preventDefault();
        const formData = new FormData(ev.nativeEvent.target)
        console.log("🚀 ~ file: identity.js:12 ~ submitEmailCheck ~ formData:", formData.get('email'))

    }
    
    async function checkEmail(email) {
        const res = await postToApi('/api/check-user', {email});
        console.log(res);
    }
    return (
        <>
        <form name="email-check-form" onSubmit={submitEmailCheck}>
            <label>Enter email:</label>
            <input name="email" type="email"></input>
            <button>Submit</button>
        </form>
        </>
    )
}