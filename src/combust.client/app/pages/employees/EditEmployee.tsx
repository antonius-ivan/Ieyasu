import * as React from "react";

import {
    Checkbox,
    Combobox,
    Field,
    Input,
    makeResetStyles,
    makeStyles,
    Radio,
    RadioGroup,
    Slider,
    SpinButton,
    Switch,
    Textarea,
    Option,
    tokens,
} from "@fluentui/react-components";
import { DatePicker } from "@fluentui/react-datepicker-compat";
import { TimePicker } from "@fluentui/react-timepicker-compat";

//const useStackClassName = makeResetStyles({
//    display: "flex",
//    flexDirection: "column",
//    rowGap: tokens.spacingVerticalL,
//});
const useClasses = makeStyles({
    dateControl: {
        maxWidth: "300px",
    },
});
export function EditEmployee(): JSX.Element {
    const classes = useClasses();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {

    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="Employee First Name" size="medium" required style={{ marginBottom: "1rem" }}>
                <Input
                    placeholder="enter the first name"
                />
            </Field>
            {/*<Field label="Employee Last Name" size="large" required style={{ marginBottom: "1rem" }}>*/}
            {/*    <Input*/}
            {/*        placeholder="enter the lastmsfkdfna name"*/}
            {/*    />*/}
            {/*</Field>*/}
            <Field label="Employee Last Name" size="large" required style={{ marginBottom: "1rem" }}>
                <Textarea
                    placeholder="enter the lastmsfkdfna name"
                />
            </Field>
            <Field label="Is Active">
                <Switch />
            </Field>
            <Field label="Select a Hireddate">
                <DatePicker
                    placeholder="Select a date..."
                />
            </Field>
            <Field label="Select a hired time">
                <TimePicker
                    placeholder="Select a time..."
                    freeform
                />
            </Field>
            <Field label="Combobox">
                <Combobox>
                    <Option>Option 1</Option>
                    <Option>Option 2</Option>
                    <Option>Option 3</Option>
                </Combobox>
            </Field>
            <Field label="SpinButton">
                <SpinButton />
            </Field>
            <Field hint="Checkboxes use their own label instead of the Field label.">
                <Checkbox label="Checkbox" />
            </Field>
            <Field label="Slider">
                <Slider defaultValue={25} />
            </Field>
            <Field label="RadioGroup">
                <RadioGroup>
                    <Radio label="Option 1" />
                    <Radio label="Option 2" />
                    <Radio label="Option 3" />
                </RadioGroup>
            </Field>
        </form>

    )
}

export default EditEmployee;
