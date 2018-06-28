import React, { PropTypes } from 'react'
import { connect } from 'react-redux';
import QueueItem from './QueueItem.jsx';

class Queue extends React.Component {
  render() {
		var queuelist = [];
		for (var i = 0; i < this.props.queue.length; i++) {

			var lineDescription = this.props.queue[i].artist;
			if (lineDescription === null || lineDescription === "") {
				lineDescription = this.props.queue[i].description;
			}
			else if (this.props.queue[i].description !== null && this.props.queue[i].description !== "") {
				lineDescription += ' - ' + this.props.queue[i].description;
			}

      queuelist.push({
				Description: lineDescription,
        TrackLength: this.props.queue[i].trackLength,
        QueueId: i
      });
    }

    return (
        <div className="body-queue-panel">
          <div className="body-queue-panel-header">
            <i className="list icon"></i> Playlist
          </div>
          <div className="body-queue-panel-content">
            {
                queuelist.map(line =>
                  <QueueItem description={line.Description} tracklength={line.TrackLength} active={line.QueueId === 0}  />
                )
            }
          </div>
        </div>
    )
  }
}

Queue.propTypes = {
}

function mapStateToProps(state) {
  return {
		queue: state.musicusState.queue
  }
}

export default connect(mapStateToProps)(Queue)
