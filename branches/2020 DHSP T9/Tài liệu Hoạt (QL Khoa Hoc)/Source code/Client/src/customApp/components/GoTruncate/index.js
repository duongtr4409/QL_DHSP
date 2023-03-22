import React from "react";
import TruncateMarkup from "react-truncate-markup";

import ReactHtmlParser from "react-html-parser";

export default class QLDeXuat extends React.Component {
  state = { shouldTruncate: true };

  toggleTruncate = () => {
    this.setState((state) => ({ shouldTruncate: !state.shouldTruncate }));
    // this.props.onToggle();
  };

  render() {
    const readMoreEllipsis = (
      <span>
        ...{" "}
        <span onClick={this.toggleTruncate} className="text-primary pointer">
          xem thêm
        </span>
      </span>
    );
    return (
      <div className="">
        {this.state.shouldTruncate ? (
          <TruncateMarkup lines={5} ellipsis={readMoreEllipsis}>
            <div style={{ width: 270 }}>{ReactHtmlParser(this.props.value)}</div>
          </TruncateMarkup>
        ) : (
          <div>
            <div dangerouslySetInnerHTML={{ __html: this.props.value }}></div>
            <span onClick={this.toggleTruncate} className="text-primary pointer">
              Thu gọn
            </span>
          </div>
        )}
      </div>
    );
  }
}
