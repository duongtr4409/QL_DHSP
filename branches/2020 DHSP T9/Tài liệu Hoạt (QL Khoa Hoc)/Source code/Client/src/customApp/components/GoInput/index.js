/**
 *
 * GoInput
 *
 */

import React from "react";
import { ValidatorComponent } from "react-form-validator-core";
import { Input, InputNumber } from "antd";
import { withInputToText } from "../withInputToText";
import lodash from "lodash";
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoInput extends ValidatorComponent {
  renderValidatorComponent() {
    const { errorMessages, validators, requiredError, validatorListener, isNumber, id, isTextArea, ...rest } = this.props;

    const { isValid } = this.state;
    const thisSelect = document.getElementById(this.props.id);

    if (this.props.withTextToInput && thisSelect) {
      thisSelect.classList.remove("text-to-input-focus");
      thisSelect.classList.add("text-to-input-blur");
    }
    if (isTextArea) {
      return (
        <div id={id}>
          <Input.TextArea
            className={isValid ? "" : "border-danger"}
            {...rest}
            ref={(r) => {
              this.input = r;
            }}
            onFocus={() => {
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-blur");
                thisSelect.classList.add("text-to-input-focus");
              }
            }}
            onBlur={() => {
              // console.log(2);
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-focus");
                thisSelect.classList.add("text-to-input-blur");
              }
            }}
          />
          {this.errorText()}
        </div>
      );
    }
    return (
      <div id={id}>
        {isNumber ? (
          <InputNumber
            {...rest}
            ref={(r) => {
              this.input = r;
            }}
            className={isValid ? "" : "border-danger"}
            formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")}
            parser={(value) => value.replace(/\$\s?|(,*)/g, "")}
            onFocus={() => {
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-blur");
                thisSelect.classList.add("text-to-input-focus");
              }
            }}
            onBlur={() => {
              // console.log(2);
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-focus");
                thisSelect.classList.add("text-to-input-blur");
              }
            }}
          />
        ) : (
          <Input
            className={isValid ? "" : "border-danger"}
            {...rest}
            ref={(r) => {
              this.input = r;
            }}
            onFocus={() => {
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-blur");
                thisSelect.classList.add("text-to-input-focus");
              }
            }}
            onBlur={() => {
              // console.log(2);
              if (this.props.withTextToInput && thisSelect) {
                thisSelect.classList.remove("text-to-input-focus");
                thisSelect.classList.add("text-to-input-blur");
              }
            }}
          />
        )}

        {this.errorText()}
      </div>
    );
  }
  errorText() {
    const { isValid } = this.state;

    if (isValid) {
      return null;
    }

    return (
      <div className="invalid-error" style={{ color: "red" }}>
        {this.getErrorMessage()}
      </div>
    );
  }
}

GoInput.propTypes = {};

export default withInputToText(GoInput);
