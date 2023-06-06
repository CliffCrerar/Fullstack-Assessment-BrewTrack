// the form builder that consumes the above and turns it into jsx

import React, { useState } from "react";
import { Button, Input, InputGroup, InputGroupText, Spinner } from "reactstrap";

/**
 * @description Element that takes input data and builds a form
 * @param {*} param0 
 * @returns 
 */
export function FormBuilder({ paramSets, onChange, formState, spinnerState, icons, buttonDisabled }) {
    const [elementFocus, setElementFocus] = useState();
    const onNativeChange = (ev) => {
        ev.preventDefault();
        ev.persist();
        onChange(ev);
    }
    const onElementFocus = (ev) => {
        setElementFocus(ev.target.name);
    }
    return paramSets.map(([type, name, text, required], idx) => {
        if (idx === paramSets.length - 1) {
            return <Button
                className="d-block m-auto mt-2"
                disabled={buttonDisabled}
                size="lg"
                key={idx}
                type={type}
                color={name}>
                {text}
                <Spinner hidden={spinnerState} size="sm"></Spinner>
            </Button>
        }
        return (
            <React.Fragment key={idx} >
                <InputGroup className="pt-3">
                    <InputGroupText htmlFor={name + type + idx} >{icons[idx]}</InputGroupText>
                    <Input
                        onFocus={onElementFocus}
                        autoFocus={name === elementFocus}
                        onChange={onNativeChange}
                        id={name + type + idx}
                        type={type}
                        name={name}
                        placeholder={text}
                        value={formState[name]}
                        required={!!required}
                    />
                </InputGroup>
                <small className="valid-feedback color-success">Validation</small>
                <small className="invalid-feedback color-danger">Validation</small>
            </React.Fragment>
        )
    })
}