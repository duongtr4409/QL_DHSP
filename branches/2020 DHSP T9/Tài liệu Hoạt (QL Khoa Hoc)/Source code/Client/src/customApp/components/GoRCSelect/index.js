/**
 *
 * GoInput
 *
 */

import React from "react";
import { ValidatorComponent } from "react-form-validator-core";
// import VirtualizedSelect from "react-virtualized-select";
import { Select } from "antd";
import api from "../../containers/Dashboard/config";
import * as lodash from "lodash";
import { withInputToText } from "../withInputToText";

import SuperSelect from "./lib/select";
const { Option } = Select;

/* eslint-disable react/prefer-stateless-function */
class GoSelect extends ValidatorComponent {
  renderOption = (item) => {
    const { customOption } = this.props;
    if (!customOption) {
      return item.text;
    }
    return customOption(item);
  };
  onChange = (value) => {
    if (this.props.mode === "multiple") {
      // const formatValue = va
      const options = lodash.intersectionBy(
        this.props.data,
        value.map((item) => ({ value: item })),

        "value"
      );
      this.props.onChange(value, options);
    } else {
      const option = this.props.data.find((d) => d.value == value);
      this.props.onChange(value, option);
    }
  };
  renderValidatorComponent() {
    const { errorMessages, validators, requiredError, validatorListener, onChange, value, hideCondition, id, ...rest } = this.props;

    let data = this.props.data || [];

    const thisSelect = document.getElementById(this.props.id);
    // console.log(this.props.id);
    if (this.props.withTextToInput && thisSelect) {
      thisSelect.classList.remove("text-to-input-focus");
      thisSelect.classList.add("text-to-input-blur");
    }

    const { isValid } = this.state;
    const parseValue = JSON.parse(value);
    const exitValue = !value ? -1 : data.findIndex((d) => d.CanBoID === parseValue.CanBoID && d.LaCanBoTrongTruong === Boolean(parseValue.LaCanBoTrongTruong));
    // console.log(parseValue, exitValue);
    // console.log(data.filter((d) => d.CanBoID === parseValue.CanBoID));
    return (
      <div id={id}>
        <SuperSelect
          allowClear
          className={isValid ? "" : "go-select-invalid"}
          value={exitValue === -1 ? null : value}
          showSearch
          filterOption
          optionFilterProp="label"
          filterOption={(input, option) => option.props.label.toUpperCase().indexOf(input.toUpperCase()) >= 0}
          onChange={this.props.returnFullItem ? this.onChange : onChange}
          data={data}
          // open
          {...rest}
          notFoundContent={<div>Không có dữ liệu</div>}
          ref={(r) => {
            this.input = r;
          }}
        >
          {data.map((d, index) => {
            // // console.log(d)
            // if (d.CanBoID === 78) {
            //   const exitCanbo = data.findIndex((item) => item.CanBoID === d.CanBoID && item.LaCanBoTrongTruong === d.LaCanBoTrongTruong);
            //   console.log(exitCanbo, d);
            // }

            return (
              <Option key={index} label={d.TenCanBo} value={data.findIndex((item) => item.CanBoID === d.CanBoID && item.LaCanBoTrongTruong === d.LaCanBoTrongTruong) !== -1 ? JSON.stringify({ CanBoID: d.CanBoID, LaCanBoTrongTruong: d.LaCanBoTrongTruong === false ? 0 : 1 }) : ""}>
                {this.renderOption(d)}
              </Option>
            );
          })}
        </SuperSelect>
        {this.errorText()}
      </div>
    );
  }
  errorText() {
    const { isValid } = this.state;

    if (isValid) {
      return null;
    }

    return <div style={{ color: "red" }}>{this.getErrorMessage()}</div>;
  }
}

export default withInputToText(GoSelect);
