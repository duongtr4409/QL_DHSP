import React from 'react';
import { TimePicker } from 'antd';

function onOpenTimePickerChange(dropdownClassName) {
  setTimeout(() => {
    const dropdownClassNameObj = document.getElementsByClassName(dropdownClassName)[0];
    if(dropdownClassNameObj){
      dropdownClassNameObj.getElementsByTagName('input')[0].addEventListener("keyup", (e) => onTextBoxTimePickerKeyUp(e, dropdownClassName));
      dropdownClassNameObj.getElementsByTagName('input')[0].maxLength = 5;
    }
  }, 500);
}
function onTextBoxTimePickerKeyUp(e, dropdownClassName) {
  const inputObj = document.getElementsByClassName(dropdownClassName)[0].getElementsByTagName('input')[0];
  let value = inputObj.value;
  if (value.match(/^\d{2}$/) !== null) {
    inputObj.value = value + ':';
  }
}

class TimePickerFormat extends React.PureComponent{
  render(){
    return <TimePicker {...this.props}
                       dropdownClassName={"ant-time-picker-panel"}
                       onOpenChange={() => onOpenTimePickerChange("ant-time-picker-panel")} />
  }
}
export default TimePickerFormat;