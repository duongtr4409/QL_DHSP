import React from "react";
import { DatePicker } from "antd";

function onOpenDatePickerChange(dropdownClassName) {
  setTimeout(() => {
    const dropdownClassNameObj = document.getElementsByClassName(dropdownClassName)[0];
    if (dropdownClassNameObj) {
      dropdownClassNameObj.getElementsByTagName("input")[0].addEventListener("keydown", (e) => onTextBoxDatePickerKeyUp(e, dropdownClassName));
      dropdownClassNameObj.getElementsByTagName("input")[0].maxLength = 10;
    }
  }, 500);
}

function onTextBoxDatePickerKeyUp(e, dropdownClassName) {
  const inputObj = document.getElementsByClassName(dropdownClassName)[0].getElementsByTagName("input")[0];
  let value = inputObj.value;
  if (value.match(/^\d{2}$/) !== null) {
    if (e.code !== "Backspace") {
      inputObj.value = value + "/";
    }
  } else if (value.match(/^\d{2}\/\d{2}$/) !== null) {
    if (e.code !== "Backspace") {
      inputObj.value = value + "/";
    }
  }
}

class DatePickerFormat extends React.PureComponent {
  render() {
    return <DatePicker {...this.props} dropdownClassName={"dropdownDatePickerDiv"} onOpenChange={() => onOpenDatePickerChange("dropdownDatePickerDiv")} format={this.props.format ? this.props.format : "DD/MM/YYYY"} />;
  }
}

export default DatePickerFormat;
