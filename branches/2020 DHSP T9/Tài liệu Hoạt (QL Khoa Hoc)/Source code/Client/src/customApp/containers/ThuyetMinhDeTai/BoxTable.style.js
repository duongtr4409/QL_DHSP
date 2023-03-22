import styled from "styled-components";
import React from 'react';
import {Spin} from 'antd';

const BoxTableWrapper = styled.div`
  position: relative;
  // min-height: 300px;
  
  .loading {
    position: absolute;
    width: 100%;
    height: 100%;  
    background: rgba(255, 255, 255, 0.6);
    z-index: 99;
    
    .spin {
      position: absolute;
      z-index: 100;
      top: 50%;
      left: 50%;
    }
  }
  
  .table-head {
    width: calc(100% - 8px);
    border-collapse: collapse;
    
    tr {
      height: 45px;
    }
    
    th {
      border: solid 1px #e6e6e6;
      text-align: center;
      font-weight: bold;
      padding: 5px;
    }
    
    td {
      border: solid 1px #e6e6e6;
      padding: 5px;
    }
  }
  
  .scroll {
    max-height: calc(100vh - 350px);
    overflow-y: scroll;
    
    table {
      width: 100%;
      border-collapse: collapse;
    
      tr {
        height: 45px;
        
        &:first-child {
          td {
            border-top: none;
          }
        }
      }
    
      th {
        border: solid 1px #e6e6e6;
        text-align: center;
        font-weight: bold;
        padding: 5px;
      }
    
      td {
        border: solid 1px #e6e6e6;
        padding: 5px;
        
        .btn-action {
          margin: 5px 0px;
        }
      }
    }
  }
  
  .clickable {
    cursor: pointer;
  }
`;

const Wrapper = props => {
  return <BoxTableWrapper {...props}>
    {props.loading ? <div className='loading'>
      <div className='spin'><Spin/></div>
    </div> : ""}
    {props.children}
  </BoxTableWrapper>;
};

export default Wrapper;