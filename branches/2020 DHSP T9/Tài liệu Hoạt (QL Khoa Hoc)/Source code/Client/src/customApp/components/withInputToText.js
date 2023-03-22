import React from "react";
import lodash from "lodash";
// Take in a component as argument WrappedComponent
export const withInputToText = (WrappedComponent) => {
  // And return another component
  class HOC extends React.PureComponent {
    render() {
      if (!this.Id) {
        this.Id = this.props.id || makeElementID(5);
      }

      const thisSelect = document.getElementById(this.Id);
      // console.log(this.props.id);
      if (this.props.withTextToInput && thisSelect) {
        thisSelect.classList.remove("text-to-input-focus");
        thisSelect.classList.add("text-to-input-blur");
      }
      return (
        <WrappedComponent
          onFocus={() => {
            if (this.props.withTextToInput && thisSelect) {
              thisSelect.classList.remove("text-to-input-blur");
              thisSelect.classList.add("text-to-input-focus");
            }
          }}
          onBlur={() => {
            if (this.props.withTextToInput && thisSelect) {
              thisSelect.classList.remove("text-to-input-focus");
              thisSelect.classList.add("text-to-input-blur");
            }
          }}
          id={this.Id}
          {...this.props}
        />
      );
    }
  }
  return HOC;
};
function makeElementID(length) {
  var result = "";
  var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  var charactersLength = characters.length;
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }
  return result;
}
