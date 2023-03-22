import React from "react";
import styled from "styled-components";
import {Popover} from "antd";

const PopoverCustom = styled(Popover)`
  .ant-popover-inner-content {
    padding: 0;
    box-shadow: 5px 5px 8px 5px #888888;
  }

  .ant-popover-placement-bottomLeft {
    padding-top: 0;
  }

  .ant-popover-arrow {
    display: none;
  }
`;

export default props => (
  <PopoverCustom {...props}>
    {props.children}
  </PopoverCustom>
);