/**
 *
 * GoInput
 *
 */

import React from "react";
import { ValidatorComponent } from "react-form-validator-core";
import DatePicker from "../../../components/uielements/datePickerFormat";
import { DatePicker as DatePickerAnt } from "antd";
import { DatePicker as DatePicker4 } from "antd4";
import locale from "antd/es/date-picker/locale/vi_VN";
// import { DatePicker } from "antd";
import { withInputToText } from "../withInputToText";
// import PropTypes from 'prop-types';
// import styled from 'styled-components';

/* eslint-disable react/prefer-stateless-function */
class GoInput extends ValidatorComponent {
  renderValidatorComponent() {
    const { useAnt, errorMessages, validators, requiredError, validatorListener, isNumber, monthPicker, id, ...rest } = this.props;
    const { isValid } = this.state;
    if (monthPicker) {
      return (
        <div id={id}>
          <DatePicker4
            picker="month"
            className={isValid ? "" : "border-danger"}
            monthCellRender={(currentDate) => {
              // console.log(currentDate);
              return <div className="ant-picker-cell-inner">T{currentDate.format("M")}</div>;
            }}
            // locale={locale}
            {...rest}
            ref={(r) => {
              this.input = r;
            }}
          />
          {this.errorText()}
        </div>
      );
    }
    if (useAnt) {
      return (
        <div id={id}>
          <DatePickerAnt
            className={isValid ? "" : "border-danger"}
            placeholder="Chọn ngày"
            {...rest}
            ref={(r) => {
              this.input = r;
            }}
          />
          {this.errorText()}
        </div>
      );
    }

    return (
      <div id={id}>
        <DatePicker
          className={isValid ? "" : "border-danger"}
          placeholder="Chọn ngày"
          {...rest}
          ref={(r) => {
            this.input = r;
          }}
        />
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
