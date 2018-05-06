# WPF.NumericInput

xmlns:ni="http://hotko.org/wpf/controls/numericinput"

<ni:NumericInputInteger Name="Edit_PodiumStairCount"
                                        Minimum="0"
                                        Maximum="40"
                                        TextAlignment="Right"
                                        VerticalAlignment="Center"
                                        HorizontalAlignment="Stretch"
                                        Margin="2"
                                        Grid.Column="2"
                                        Grid.Row="3" />
										
_result.PodiumStairsCount = Edit_PodiumStairCount.Value??0;